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
using TMPro;
using System.IO;
using System;
using UnityEngine.Events;
using System.Collections;

namespace Ravioli.Music
{
    /// <summary>
    /// 
    /// </summary>
    public class PrepareTextSequence : MonoBehaviour
    {
        #region Events
        public UnityEvent OnNextWord;
        #endregion Events
        #region [SerializeField] Private Members
        [SerializeField]
        private TextMeshProUGUI _textInScene;
        [SerializeField]
        private string _songTitle;
        [SerializeField]
        private string _lyricsTime;
        [SerializeField]
        private string _folderName;
        [SerializeField, Tooltip("Enable to play the handmade animation on start, do not use it in the process of animating.")]
        private bool _baked;
        #endregion [SerializeField] Private Members
        #region Private Members
        private string[] _words;
        private int _currentWordIndex;
        #endregion Private Members
        #region Monobehaviour
        private void Start()
        {
            // Load the text file and split it into words
            string filePath = GetFile(_folderName, _songTitle);
            string fileContents = File.ReadAllText(filePath);
            _words = fileContents.Split(new char[] { ' ', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

            // Start at the first word
            _currentWordIndex = 0;
            DisplayCurrentWord();
            if (_baked)
            {
                StartCoroutine(WaitForTimeInFileCoroutine());
            }
        }

        private void Update()
        {
            // you will need this only in the editor 
            if (Input.GetKeyDown(KeyCode.Space) && !_baked)
            {
                NextWord();
            }
        }
        #endregion Monobehaviour
        #region Private Methods
        private void NextWord()
        {
            // Increment the current word index
            _currentWordIndex++;

            // If we've reached the end of the text, wrap around to the beginning
            if (_currentWordIndex >= _words.Length)
            {
                _currentWordIndex = 0;
            }

            // Display the new current word
            DisplayCurrentWord();
            OnNextWord.Invoke();
        }
        /// <summary>
        /// 
        /// </summary>
        private void DisplayCurrentWord()
        {
            // Display the current word in the text mesh
            _textInScene.text = _words[_currentWordIndex];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetFile(string folderName, string fileName)
        {
            string folderPath = Path.Combine(Application.dataPath, folderName);
            string[] files = Directory.GetFiles(folderPath, fileName);

            if (files.Length > 0)
            {
                return files[0];
            }
            else
            {
                Debug.LogError("File not found: " + Path.Combine(folderPath, fileName));
                return null;
            }
        }
        #endregion Private Methods
        int wordIndex = -1;
        float timeToWait = 0f;

        #region Coroutines
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator WaitForTimeInFileCoroutine()
        {
            string filePath = GetFile(_folderName, _lyricsTime);
            string fileContents = File.ReadAllText(filePath);
            wordIndex++;
            string[] words = fileContents.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            timeToWait = float.Parse(words[wordIndex]);
            yield return new WaitForSecondsRealtime(timeToWait);
            Debug.Log(timeToWait);
            NextWord();
            StartCoroutine(WaitForTimeInFileCoroutine());
        }

        #endregion Coroutines
    }
}
