using System;
using System.Collections;
using System.Collections.Generic;
using Extend;
using TMPro;
using UnityEngine;

namespace GameThing
{
    public class GameCountDownMenuUi : MonoBehaviour
    {
        public float timeToCountDown = 3f;
        public float countDownInterval = 1f;
        public TextMeshProUGUI contentText;
         
        public void StartCountDown()
        {
            gameObject.SetActive(true);
            StartCoroutine(CountDown());
        }

        IEnumerator CountDown()
        { 
            float currentTime = timeToCountDown;
            UpdateContentText("Loading...");
            while (currentTime > 0)
            {
                UpdateContentText(currentTime.ToString());
                yield return new WaitForSeconds(countDownInterval);
                currentTime -= countDownInterval;
            }
            UpdateContentText("Go!");
            yield return new WaitForSeconds(countDownInterval);
            gameObject.SetActive(false);
        }

        private void UpdateContentText(string content)
        {
            contentText.text = content;
        }
    }
    
}
