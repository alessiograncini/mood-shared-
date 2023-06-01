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
using Ravioli.Data;

namespace Ravioli.Runtime
{
    /// <summary>
    /// This class aims to load a texture to a game object materual form a folder inside the Application.DataPath
    /// </summary>
    public class TextureLoader : MonoBehaviour
    {
        #region [SerializeField] Private Members 
        [SerializeField]
        private SceneLayer[] _sceneLayers;
        [SerializeField]
        private bool _debug;
        #endregion [SerializeField] Private Members
        #region Private Members 
        private string LOCALPATHIMAGES;
        private Texture2D _texture;
        #endregion Private Members
        #region MonoBehaviour
        private void Start()
        {
            // Set the path of the folder where the images are stored
            LOCALPATHIMAGES = JSONLoader.ConfigData.Paths.IMAGES;
        }
        #endregion MonoBehaviour
        #region Public Methods
        [ContextMenu(" SetTextureFromFolder")]
        /// <summary>
        /// 
        /// </summary>
        public void SetTextureFromFolder()
        {
            // Construct the file path of the image based on the application data path
            string localPath = Path.Combine(Application.dataPath, "_LocalData/Images");

            // Get the first .jpg file in the folder
            string[] files = Directory.GetFiles(localPath, "*.png");

            if (files.Length > 0)
            {
                string firstFilePath = files[0];
                Debug.Log("First file name: " + firstFilePath);
                byte[] imageBytes = File.ReadAllBytes(firstFilePath);
                _texture = new Texture2D(2, 2);
                _texture.LoadImage(imageBytes);

                for (int i = 0; i < _sceneLayers.Length; i++)
                {
                    if (_sceneLayers[i].SceneLayerInfo.Selected)
                    {
                        _sceneLayers[i].GetMaterial().mainTexture = _texture;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeselectAllScenes(){
            for (int i = 0; i < _sceneLayers.Length; i++)
            {
                if (_sceneLayers[i].SceneLayerInfo.Selected)
                {
                    _sceneLayers[i].DeselectLayer();
                }
            }
        }
        #endregion Public Methods
    }
}
