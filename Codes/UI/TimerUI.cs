using System;
using _01_Scripts.Core;
using TMPro;
using UnityEngine;

namespace _01_Scripts.UI
{
    public class TimerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;

        private void Update()
        {
            timerText.text = $"{TimerManager.Instance.Minutes:D2} : {Mathf.FloorToInt(TimerManager.Instance.Count):D2}";
        }
    }
}