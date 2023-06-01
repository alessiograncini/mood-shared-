/***************************************************************************************
 * Copyright (c) 2023, [wip company]. All rights reserved.
 * 
 * This software is the confidential and proprietary information of [company name].
 * You shall not disclose or reproduce this software, in whole or in part, without 
 * the prior written consent of [company name]. 
 * 
 * This software is provided "as is," without warranty of any kind, express or implied,
 * including but not limited to the warranties of merchantability, fitness for a particular
 * purpose and noninfringement. In no event shall [wip company] be liable for any claim,
 * damages or other liability arising from the use of this software. 
 *
 * For inquiries about the use of this software, please contact alessio.grancini@gmail.com
 ***************************************************************************************/

using UnityEngine;
using System.Collections;
using Ravioli.Web;
using Ravioli.Amazon;
using TMPro;
using Ravioli.Utilities;

namespace Ravioli.Runtime
{
    /// <summary>
    /// Curates the flow for generating the image in x steps 
    /// 1. Get prompt
    /// .upload one copy on a S3 bucket that store a temporary copy of prompt
    /// .upload one copy on a S3 bucket that store a long term copy of prompt
    /// 2. Process Stable Diffusion Generation with prompt 
    /// 3. Apply Image to selected texture in scene 
    /// </summary>
    [RequireComponent(typeof(TextureLoader))]
    [RequireComponent(typeof(WebRequest))]
    [RequireComponent(typeof(S3Manager))]
    public class GenerateImage : MonoBehaviour
    {
        #region Private Members [SerializeField]
        [SerializeField] TextMeshProUGUI _promptText;
        [SerializeField] float _timeBetweenSteps = 10f;
        [SerializeField] RadialProgress _radialProgress;
        [SerializeField] bool _debug;
        #endregion Private Members [SerializeField]
        #region Private Members
        private TextureLoader _textureLoader;
        private WebRequest _webRequest;
        private S3Manager _s3Manager;

        #endregion Private Members 

        #region Monobehaviour 
        private void OnEnable()
        {
            Initialize();
        }
        #endregion Monobehaviour
        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        public void GenerateScene()
        {
            StopAllCoroutines();
            StartCoroutine(GenerateSceneViaPrompt(_timeBetweenSteps));
        }


        [ContextMenu("Test: Send Prompt")]
        public void TestSendPrompt()
        {
            _s3Manager.SendPrompts(_promptText.text);
        }
        [ContextMenu("Test: WebRequest execute")]
        public void TestExecute()
        {
            _webRequest.Execute();
        }
        [ContextMenu("Test: Download Scene")]
        public void TestDownloadScene()
        {
            _s3Manager.DownloadScene();
        }
        [ContextMenu("Test: Set Texture")]
        public void TestSetTexture()
        {
            _textureLoader.SetTextureFromFolder();
        }

        #endregion Public Methods



        #region Coroutines
        /// <summary>
        /// Reccomended to test this flow one step at a time 
        /// </summary>
        IEnumerator GenerateSceneViaPrompt(float waitTime)
        {
            _radialProgress.StartProgressBar();
            // send prompt to S3 
            _s3Manager.SendPrompts(_promptText.text);
            yield return new WaitForSeconds(waitTime);
            if (_debug)
            {
                Debug.Log("Prompt sent to S3");
            }
            // execute python script
            _webRequest.Execute();
            yield return new WaitForSeconds(waitTime);
            if (_debug)
            {
                Debug.Log("Executed Python Script");
            }
            // download Imahe 
            _s3Manager.DownloadScene();
            yield return new WaitForSeconds(waitTime);
            if (_debug)
            {
                Debug.Log("Downloaded Image");
            }
            // apply generated texture to selected scene object 
            _textureLoader.SetTextureFromFolder();
            if (_debug)
            {
                Debug.Log("Set Texture");
            }
            yield return null;
        }
        #endregion Coroutines

        #region Private Methods
        private void Initialize()
        {
            _textureLoader = GetComponent<TextureLoader>();
            _s3Manager = GetComponent<S3Manager>();
            _webRequest = GetComponent<WebRequest>();
        }
        #endregion Private MethodDS
    }
}
