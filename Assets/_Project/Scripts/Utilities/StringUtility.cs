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
using System.IO;
using System;
using System.Text;


namespace Ravioli.Utilities
{
    /// <summary>
    /// String utilities class
    /// </summary>

    public static class StringUtility
    {
        /// <summary>
        ///
        /// </summary>
        public static void WriteToFile(string text, string folder, string file)
        {
            string path = Path.Combine(Application.dataPath, folder, file);
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(text);
            }
        }
        /// <summary>
        ///
        /// </summary>
        public static void CreateFile(string folder, string file)
        {
            try
            {
                string folderPath = Path.Combine(Application.dataPath, folder);

                // Check if the folder exists, and if not, create it
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, file);
                File.Create(filePath).Close();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating file: {ex.Message}");
            }
        }
        /// <summary>
        ///
        /// </summary>
        public static string GenerateRandomString(int length)
        {
            const string chars = "0123456789";
            var random = new System.Random();
            var result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }
            return result.ToString();
        }


    }
}