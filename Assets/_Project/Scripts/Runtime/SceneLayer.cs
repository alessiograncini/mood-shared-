using UnityEngine;
using System;
using System.Collections;


namespace Ravioli.Runtime
{
    /// <summary>
    /// Tracking informations on scene layer 
    /// </summary>
    public class SceneLayer : MonoBehaviour
    {
        #region Nested Classes
        [Serializable]
        public class Info
        {
            public bool Selected;
            // To be referenced in the inspector 
            public MeshRenderer MeshRenderer;
        }
        #endregion Nested Classes

        #region Public Members
        public Info SceneLayerInfo;
        #endregion Public Members

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        public void SelectLayer()
        {
            StartCoroutine(SelectDelay());
        }
        /// <summary>
        /// 
        /// </summary>
        public void DeselectLayer()
        {
            SceneLayerInfo.Selected = false;
        }
        /// <summary>
        /// 
        /// </summary>
        public Material GetMaterial()
        {
            return SceneLayerInfo.MeshRenderer.material;
        }
        /// <summary>
        /// 
        /// </summary>
        public Texture GetTexture()
        {
            return SceneLayerInfo.MeshRenderer.material.GetTexture("_MainTex");
        }
        /// <summary>
        /// 
        /// </summary>
        public void SetTexture(Texture texture)
        {
            SceneLayerInfo.MeshRenderer.material.SetTexture("_MainTex", texture);
        }
        #endregion Public Methods
        #region Coroutines
        IEnumerator SelectDelay()
        {
            yield return new WaitForSeconds(0.2f);
            SceneLayerInfo.Selected = true;
        }
        #endregion Coroutines
    }

}