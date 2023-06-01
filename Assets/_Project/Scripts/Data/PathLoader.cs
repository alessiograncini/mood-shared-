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

namespace Ravioli.Data
{
    public static class PathLoader
    {
        public static class Bucket
        {
            public static string BUCKET_NAME_SCENE_TEMP = "stablediffusion-scene-store";
            public static string BUCKET_NAME_SCENE_STORE = "stablediffusion-scene-store";
            public static string BUCKET_NAME_PROMPT_TEMP = "stablediffusion-prompt";
            public static string BUCKET_NAME_PROMPT_STORE = "stablediffusion-prompt-store";
        }

        public static class Credentials
        {
            public static string ACCESS_KEY = "AKIATHLYHJJEJZ5TO42U";
            public static string SECRET_KEY = "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";
        }

        public static class Paths
        {
            public static string IMAGES = "_LocalData/Images";
            public static string PROMPTS = "_LocalData/Prompts";
        }

        public static class Web
        {
            public static string SERVERURL = "https://192.168.1.180:8000";
            public static string RUNPYTHON = "/home/alessio/Dev/ravioli-backend/run_python.sh";
            public static string STOPPYTHON = "/home/alessio/Dev/ravioli-backend/kill_python.sh";
        }


    }
}
