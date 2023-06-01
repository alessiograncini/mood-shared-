/***************************************************************************************
 * Copyright (c) [year], [company name]. All rights reserved.
 * 
 * This software is the confidential and proprietary information of [company name].
 * You shall not disclose or reproduce this software, in whole or in part, without 
 * the prior written consent of [company name]. 
 * 
 * This software is provided "as is," without warranty of any kind, express or implied,
 * including but not limited to the warranties of merchantability, fitness for a particular
 * purpose and noninfringement. In no event shall [company name] be liable for any claim,
 * damages or other liability arising from the use of this software. 
 *
 * For inquiries about the use of this software, please contact [contact email].
 ***************************************************************************************/

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Ravioli.Data;
using System.Diagnostics;
using System;
using System.Text.RegularExpressions;
using System.Threading;


using System.Collections.Concurrent;

namespace Ravioli.Web
{
    public class InsecureCertificateHandler : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            // Bypass SSL certificate validation by returning true for all certificates
            return true;
        }
    }
    /// <summary>
    ///
    /// </summary>
    public class WebRequest : MonoBehaviour
    {

        private ConcurrentQueue<string> logQueue = new ConcurrentQueue<string>();
        #region Private Members
        private Dispatcher dispatcher;
        private WebRequest _webRequest;
        #endregion Private Members
        #region Private Methods

        private void Awake()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
        }


        [ContextMenu("Start")]
        public void Execute()
        {
            StopAllCoroutines();
            StartCoroutine(ExecuteScript());
            StartCoroutine(FetchProgressCoroutine());
        }
        [ContextMenu("Stop")]
        public void Stop()
        {
            StopAllCoroutines();
            StartCoroutine(StopScript());
        }
        #endregion Private Methods


        #region Coroutines
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator ExecuteScript()
        {
            // TODO: understand why when youy refenece the path from the PathLoader or JsonLoader it doesn't work
            // also sometimes it works, sometimes it doesn't http://192.168.1.180:8081 vs https://192.168.1.180:8081
            // something you can try if things don't work 
            // .reboot Ubuntu PC 
            // .change port 
            // .change http to https and viceversa
            UnityWebRequest www = UnityWebRequest.Get("YOUR-ADDRESS" + "/execute?script-path=" + "YOUR-SERVER-LOCATION");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                UnityEngine.Debug.Log("Server pinged successfully!");
            }
            else
            {
                UnityEngine.Debug.Log("Failed to ping server: " + www.error);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator StopScript()
        {
            string url = PathLoader.Web.SERVERURL + "YOUR-SERVER-LOCATION" + PathLoader.Web.STOPPYTHON;
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    UnityEngine.Debug.LogError($"Error: {request.error}");
                }
                else
                {
                    UnityEngine.Debug.Log("Script stopped successfully.");
                }
            }
        }
        public IEnumerator FetchProgressCoroutine()
        {
            while (true)
            {
                UnityWebRequest www = UnityWebRequest.Get("YOUR-SERVER-LOCATION/progress");
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    if (!string.IsNullOrWhiteSpace(www.downloadHandler.text))
                    {
                        try
                        {
                            float progress = float.Parse(www.downloadHandler.text);
                            UnityEngine.Debug.Log($"Progress: {progress}%");
                        }
                        catch (FormatException e)
                        {
                            UnityEngine.Debug.LogError($"Failed to parse progress: '{www.downloadHandler.text}', Error: {e.Message}");
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.LogWarning("Server returned an empty progress value.");
                    }
                }
                else
                {
                    UnityEngine.Debug.Log("Failed to fetch progress: " + www.error);
                }

                yield return new WaitForSeconds(1); // Fetch progress every second
            }
        }

        #endregion Coroutines

    }
}

