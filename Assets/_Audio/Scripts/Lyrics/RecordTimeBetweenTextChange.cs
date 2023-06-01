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

using System.Collections;
using UnityEngine;
using System.IO;

namespace Ravioli.Music
{
    /// <summary>
    /// Records the time elapsed between changes in a text sequence and writes it to a file.
    /// </summary>
    public class RecordTimeBetweenTextChange : MonoBehaviour
    {
        #region [SerializeField] Private Members
        [SerializeField]
        private string _fileName = "DalleHandTime.txt";
        [SerializeField]
        private string _folderName = "_LocalData/Lyrics";
        [SerializeField, Tooltip("Needed to use this class, only use in editor mode to register the text animation")]
        private bool _activate;
        #endregion [SerializeField] Private Members
        #region Private Members
        private float _time;
        #endregion Private Members
        #region Monobehaviour
        private void Awake()
        {
            if (_activate)
            {
                string folderPath = Path.Combine(Application.dataPath, _folderName);
                string filePath = Path.Combine(folderPath, _fileName);

                // If the file already exists, delete its content
                if (File.Exists(filePath))
                {
                    File.WriteAllText(filePath, "");
                }
            }
        }
        private void Update()
        {
            if (_activate)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    RecordTimeForChanges();
                }
            }
        }
        #endregion Monobehaviour
        #region Public Methods
        [ContextMenu("RecordTimeForChanges()")]
        public void RecordTimeForChanges()
        {
            Write();
            StopAllCoroutines();
            StartCoroutine(RecordTimeForChangesCoroutine(_folderName, _fileName));
        }
        #endregion Public Methods
        #region Private Methods
        private void Write()
        {
            string folderPath = Path.Combine(Application.dataPath, _folderName);
            string filePath = Path.Combine(folderPath, _fileName);
            StreamWriter writer = new StreamWriter(filePath, true);
            writer.Write(_time.ToString("0.00"+ " "));
            writer.Close();
        }
        #endregion Private Methods
        #region Coroutines
        /// <summary>
        /// Records a keyframe for the specified boolName in the Animator.
        /// </summary>
        private IEnumerator RecordTimeForChangesCoroutine(string folderName, string fileName)
        {
            _time = 0;

            while (true)
            {
                _time += Time.deltaTime;
                yield return null;
            }
        }
        #endregion Coroutines
    }
}
