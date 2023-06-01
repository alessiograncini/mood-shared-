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

using TMPro;
using UnityEngine;

namespace Ravioli.Utilities
{
    /// <summary>
    /// String utilities class
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class ExpandRectBasedOnTextLines : MonoBehaviour
    {
        #region Private Members [SerializedFields]
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        [SerializeField] private float _lineHeight = 1.2f; // Set this to match your TextMeshPro line height
        #endregion Private Members [SerializedFields]
        #region Private Members 
        private RectTransform _rectTransform;
        #endregion Private Members 
        #region Monobehaviour 
        private void Start()
        {
              if (GetComponent<RectTransform>() != null){
             _rectTransform = GetComponent<RectTransform>();
             }
            UpdateRectTransform();
        }

        private void Update()
        {
             if (GetComponent<RectTransform>() != null){
             _rectTransform = GetComponent<RectTransform>();
             }

            if (_textMeshPro.text != null && _textMeshPro.text != "")
            {
                UpdateRectTransform();
            }
        }
        #endregion Monobehaviour 
        #region Private Methods
        private void UpdateRectTransform()
        {
            float textHeight = _textMeshPro.preferredHeight;
            int numLines = Mathf.CeilToInt(textHeight / _lineHeight);
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _lineHeight * numLines);
        }

        #endregion Private Methods
    }

}
