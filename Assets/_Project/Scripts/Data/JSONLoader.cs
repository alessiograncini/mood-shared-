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

using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Ravioli.Data
{

    /// <summary>
    ///
    /// </summary>
    public class JSONLoader : MonoBehaviour
    {

        public static Config ConfigData { get; private set; }

        private void Awake()
        {
            LoadJsonFile();
        }

        private static void LoadJsonFile()
        {
            string filePath = Path.Combine(Path.Combine(Application.dataPath, "_LocalData", "Json", "Data.json"));

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                ConfigData = JsonUtility.FromJson<Config>(dataAsJson);

                // Access and use the strings as needed
                Debug.Log("Bucket name: " + ConfigData.AmazonBucket.BUCKET_NAME_SCENE_TEMP);
            }
            else
            {
                Debug.LogError("Cannot find JSON file!");
            }
        }
        [System.Serializable]
        public class AmazonBucket
        {
            public string BUCKET_NAME_SCENE_TEMP;
            public string BUCKET_NAME_SCENE_STORE;
            public string BUCKET_NAME_PROMPT_TEMP;
            public string BUCKET_NAME_PROMPT_STORE;
        }

        [System.Serializable]
        public class Credentials
        {
            public string ACCESS_KEY;
            public string SECRET_KEY;
        }

        [System.Serializable]
        public class Paths
        {
            public string IMAGES;
            public string PROMPTS;
        }

        [System.Serializable]
        public class Web
        {
            public string SERVERURL;
            public string RUNPYTHON;
            public string STOPPYTHON;
        }

        [System.Serializable]
        public class Config
        {
            public AmazonBucket AmazonBucket;
            public Credentials Credentials;
            public Paths Paths;
            public Web WEB;
        }
    }
}



