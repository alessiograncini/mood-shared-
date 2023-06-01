/***************************************************************************************
 * Copyright (c) 2023, RAVIOLI. All rights reserved.
 * 
 * This software is the confidential and proprietary information of [company name].
 * You shall not disclose or reproduce this software, in whole or in part, without 
 * the prior written consent of [company name]. 
 * 
 * This software is provided "as is," without warranty of any kind, express or implied,
 * including but not limited to the warranties of merchantability, fitness for a particular
 * purpose and noninfringement. In no event shall RAVIOLI be liable for any claim,
 * damages or other liability arising from the use of this software. 
 *
 * For inquiries about the use of this software, please contact alessio.grancini@gmail.com
 ***************************************************************************************/

using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.Linq;
using Ravioli.Data;
using System.Text;
using Ravioli.Utilities;

namespace Ravioli.Amazon
{
    /// <summary>
    /// This class aims to organize and call the tasks performed on AWS 
    /// </summary>
    public class S3Manager : MonoBehaviour
    {
        #region Private Members [SerializeField]
        [SerializeField]
        private bool _debug;
        #endregion Private Members [SerializeField]
        #region Private Members
        private AmazonS3Client _s3Client;
        #endregion Private Members
        #region Monobehaviour

        private void Start()
        {
            // Create an instance of the AmazonS3Client class
            _s3Client = new AmazonS3Client(PathLoader.Credentials.ACCESS_KEY, PathLoader.Credentials.SECRET_KEY, RegionEndpoint.USWest1);
        }

        private void OnDestroy()
        {
            // Clean up the resources used by the AmazonS3Client instance
            _s3Client.Dispose();
        }
        #endregion Monobehaviour
        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("DownloadImage")]
        public void DownloadScene()
        {
            DownloadImage(PathLoader.Bucket.BUCKET_NAME_SCENE_TEMP);
        }


        public async void DownloadImage(string bucketName)
        {
            // before to download the image clean up the local folder 
            ClearDataPathFolder(PathLoader.Paths.IMAGES);
            // Call the ListObjectsAsync method to list all the objects in the bucket asynchronously
            ListObjectsRequest request = new ListObjectsRequest
            {
                BucketName = bucketName
            };
            Task<ListObjectsResponse> task = _s3Client.ListObjectsAsync(request);
            //await task.ContinueWith(HandleListObjectsResponseAsImage(bucketName));
            await task.ContinueWith(responseTask => HandleListObjectsResponseAsImage(responseTask, bucketName));
            // if  the image is downloaded clear the bucket
            await ClearS3Bucket(bucketName);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendPrompts(string textPrompt)
        {
            SendPrompt(textPrompt, PathLoader.Bucket.BUCKET_NAME_PROMPT_TEMP, PathLoader.Bucket.BUCKET_NAME_PROMPT_STORE);
        }

        /// <summary>
        /// 
        /// </summary>
        public async void SendPrompt(string textPrompt, string instantBucket, string storeBucket)
        {
            // make sure the instan bucket is clear 
            Debug.Log("Clearing the bucket: " + instantBucket);
            await ClearS3Bucket(instantBucket);
            // create a random id name for the prompt document 

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string promptTitle = $"Prompt{timestamp}.txt";
            // create a document locally 
            StringUtility.CreateFile(PathLoader.Paths.PROMPTS, promptTitle);
            // paste inside of it the text that the user wrote 
            StringUtility.WriteToFile(textPrompt, PathLoader.Paths.PROMPTS, promptTitle);
            // upload this to s3 bucket
            await UploadTextFileToBucket(promptTitle, promptTitle, instantBucket);
            await UploadTextFileToBucket(promptTitle, promptTitle, storeBucket);
            Task uploadResponse = UploadTextFileToBucket(promptTitle, promptTitle, instantBucket);

            if (uploadResponse.IsCompleted)
            {
                Debug.Log($"Successfully uploaded {promptTitle} to {instantBucket}.");
            }
            else
            {
                Debug.LogError($"Failed to upload {promptTitle} to {instantBucket}. Error: {uploadResponse.Exception?.Message}");
            }

            // after sending a prompt clean the folder that contains the prompt file locally 
            ClearDataPathFolder(PathLoader.Paths.PROMPTS);
        }
        #endregion Public Methods


        #region Private Methods
        /// <summary>
        /// Send text from Unity to AWS3
        /// Note: Make sure that there is only one text at the time to get
        /// empty the text folder first and also the bucket 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public async Task UploadTextFileToBucket(string fileName, string keyName, string bucketName)
        {
            try
            {
                string filePath = Path.Combine(Application.dataPath, PathLoader.Paths.PROMPTS, fileName);
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    InputStream = fileStream
                };
                var response = await _s3Client.PutObjectAsync(putRequest);
                if (_debug)
                {
                    Debug.Log($"File uploaded to S3. ETag: {response.ETag}");
                }
            }
            catch (AmazonS3Exception ex)
            {
                if (_debug)
                {
                    Debug.LogError($"Error uploading file: {ex.Message}");
                }
            }
        }
        /// <summary>
        /// Get Image from AWS3 to Unity
        /// Note: Make sure that there is only one image at the time to get
        /// empty the image folder first and also the bucket 
        /// </summary>
        /// <param name="task"></param>
        private void HandleListObjectsResponseAsImage(Task<ListObjectsResponse> task, string bucketName)
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                if (_debug)
                {
                    Debug.LogError($"Error retrieving list of objects: {task.Exception}");
                }
                return;
            }

            ListObjectsResponse response = task.Result;

            foreach (S3Object obj in response.S3Objects)
            {
                Debug.Log(obj.Key);

                // this task download the image 
                if (obj.Key.EndsWith(".jpg") || obj.Key.EndsWith(".png"))
                {
                    // Download the image and save it to a file
                    Task<GetObjectResponse> downloadTask = _s3Client.GetObjectAsync(bucketName, obj.Key);
                    downloadTask.ContinueWith((downloadTask) =>
                    {
                        if (downloadTask.IsFaulted || downloadTask.IsCanceled)
                        {
                            if (_debug)
                            {
                                Debug.LogError($"Error downloading image: {downloadTask.Exception}");
                            }
                            return;
                        }

                        GetObjectResponse downloadResponse = downloadTask.Result;

                        // Construct a path to save the downloaded image relative to the application's data path
                        string localPath = Path.Combine(Application.dataPath, PathLoader.Paths.IMAGES, obj.Key);

                        // Create the directory if it doesn't exist
                        Directory.CreateDirectory(Path.GetDirectoryName(localPath));

                        // Save the downloaded image to the file
                        using (Stream outputStream = File.Open(localPath, FileMode.Create))
                        {
                            downloadResponse.ResponseStream.CopyTo(outputStream);
                        }
                        if (_debug)
                        {
                            Debug.Log($"Downloaded image {obj.Key} to {localPath}");
                        }

                    });
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private void ClearDataPathFolder(string path)
        {
            string selectedPath = Path.Combine(Application.dataPath, path);
            DirectoryInfo dataPathDir = new DirectoryInfo(selectedPath);
            FileInfo[] files = dataPathDir.GetFiles();
            foreach (FileInfo file in files)
            {
                file.Delete();
            }
            DirectoryInfo[] dirs = dataPathDir.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                dir.Delete(true);
            }
            Debug.Log("Data path folder cleared.");
        }
        /// <summary>
        /// 
        /// </summary>  
        public async Task ClearS3Bucket(string bucketName)
        {
            try
            {
                if (string.IsNullOrEmpty(bucketName))
                {
                    throw new ArgumentException("Bucket name cannot be null or empty.", nameof(bucketName));
                }
                var listRequest = new ListObjectsRequest
                {
                    BucketName = bucketName
                };
                ListObjectsResponse listResponse;
                do
                {
                    listResponse = await _s3Client.ListObjectsAsync(listRequest);

                    foreach (var s3Object in listResponse.S3Objects)
                    {
                        var deleteRequest = new DeleteObjectRequest
                        {
                            BucketName = bucketName,
                            Key = s3Object.Key
                        };
                        await _s3Client.DeleteObjectAsync(deleteRequest);
                    }

                    listRequest.Marker = listResponse.NextMarker;
                } while (listResponse.IsTruncated);
                Debug.Log("S3 bucket cleared.");
            }
            catch (AmazonS3Exception ex)
            {
                Debug.LogError($"Error clearing S3 bucket: {ex.Message}");
            }
        }
        #endregion Private Methods
    }
}