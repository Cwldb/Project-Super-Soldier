using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace _01_Scripts.UI
{
    public class Title : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDuration;

        public void GameStart()
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.DOFade(1, fadeDuration)
                .OnComplete(() => SceneManager.LoadScene("InGameScene"));
        }
    }
}