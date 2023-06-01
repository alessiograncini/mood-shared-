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
using UnityEngine.UI;
using System.Collections;

namespace Ravioli.Utilities
{
    /// <summary>
    /// Radial progress bar class
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class RadialProgress : MonoBehaviour
    {
        #region Private Members [SerializedFields]
        [SerializeField]
        private GameObject _loadingText;
        [SerializeField]
        private TextMeshProUGUI _progressIndicator;
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _timeToCompleteLoading;
        [SerializeField, Tooltip("If true, the loading time will be fixed, otherwise it will be calculated based on the speed")]
        private bool _fixedTime;
        #endregion Private Members [SerializedFields]
        #region Private Members 
        private float _currentValue;
        private bool _loadingIsEnabled;
        private Image _loadingBar;
        #endregion Private Members

        #region Monobehaviour

        private void Awake()
        {
            _loadingBar = GetComponent<Image>();
        }
        void Update()
        {
            if (!_fixedTime)
            {


                if (!_loadingIsEnabled)
                    return;

                if (_currentValue < 100)
                {
                    _currentValue += _speed * Time.deltaTime;
                    _progressIndicator.text = ((int)_currentValue).ToString() + "%";
                    _loadingText.SetActive(true);
                }
                else
                {
                    _loadingText.SetActive(false);
                    _progressIndicator.text = "Done";
                }

                _loadingBar.fillAmount = _currentValue / 100;
            }

        }
        #endregion Monobehaviour
        #region Public Methods
        /// <summary>
        /// Reset the progress bar
        /// </summary>
        public void Reset()
        {
            _currentValue = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public void StartProgressBar()
        {
            if (!_fixedTime)
            {
                Reset();
                _loadingIsEnabled = true;
            }
            else
            {
                StartProgressBarCoroutine(_timeToCompleteLoading);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void StopProgressBar()
        {
            if (!_fixedTime)
            {
                {
                    _loadingIsEnabled = false;
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public void StartProgressBarCoroutine(float fillTime)
        {
            StartCoroutine(FillProgressBar(fillTime));
        }


        #endregion Public Methods  
        #region Coroutines
        IEnumerator FillProgressBar(float fillTime)
        {
            _loadingText.SetActive(true);

            float timer = 0f;

            while (timer < fillTime)
            {
                timer += Time.deltaTime;
                _currentValue = Mathf.Lerp(0, 100, timer / fillTime);
                _progressIndicator.text = ((int)_currentValue).ToString() + "%";
                _loadingBar.fillAmount = _currentValue / 100;

                yield return null;
            }

            _currentValue = 100f;
            _progressIndicator.text = "Done";
            _loadingText.SetActive(false);
        }
        #endregion Coroutines

    }
}


