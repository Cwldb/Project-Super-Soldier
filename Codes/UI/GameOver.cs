using System;
using System.Threading;
using _01_Scripts.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _01_Scripts.UI
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] private TMP_Text endText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button exitButton;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += HandleGameEnd;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= HandleGameEnd;
        }

        private void HandleGameEnd(Scene arg0, LoadSceneMode arg1)
        {
            Debug.Log("Game Over!");
            descriptionText.text = $"당신은 {TimerManager.Instance.Minutes:D2} : {Mathf.FloorToInt(TimerManager.Instance.Count):D2} 만큼 살아남았습니다!";
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
