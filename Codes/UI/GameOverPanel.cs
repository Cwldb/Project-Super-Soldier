using System;
using _01_Scripts.Combat;
using _01_Scripts.Core;
using _01_Scripts.Players;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _01_Scripts.UI
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private GameObject overPanel;
        [SerializeField] private Image fadeImage;
        
        [SerializeField] private TMP_Text endText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button exitButton;

        private void Start()
        {
            GameManager.Instance.OnGameOver += HandleGameOver;
        }

        private void HandleGameOver()
        {
            Time.timeScale = 0;
            fadeImage.gameObject.SetActive(true);
            fadeImage.DOFade(1, 0.7f).SetUpdate(true).OnComplete(() =>
            {
                overPanel.SetActive(true);
                fadeImage.gameObject.SetActive(false);
                descriptionText.text = $"당신은 {TimerManager.Instance.Minutes:D2} : {Mathf.FloorToInt(TimerManager.Instance.Count):D2} 만큼 살아남았습니다!";
            });
        }
        
        public void RestartGame()
        {
            TimerManager.Instance.Minutes = 0;
            TimerManager.Instance.Count = 0;
            TimerManager.Instance.isDeath = false;
            TimerManager.Instance.isBossSpawn = false;
            TimerManager.Instance.is1Minute = false;
            TimerManager.Instance.is2Minute = false;
            TimerManager.Instance.is3Minute = false;
            TimerManager.Instance.is4Minute = false;
            TimerManager.Instance.is5Minute = false;
            Time.timeScale = 1;
            DOTween.KillAll();
            SceneManager.LoadScene(1);

            // GameManager.Instance.PlayerFinder.Target.transform.position = new Vector3(0, 0, 0);
            // Player p = GameManager.Instance.PlayerFinder.Target as Player;
            // p.ChangeState("IDLE");
            // p.IsDead = false;
            // p.GetCompo<EntityHealth>().maxHealth = 100;
            // p.GetCompo<EntityHealth>().currentHealth = 100;
            // p.GetCompo<CharacterMovement>().CanManualMovement = true;
            // p.GetCompo<PlayerStat>().AfterInitialize();
            // overPanel.SetActive(false);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}