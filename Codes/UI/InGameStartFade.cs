using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _01_Scripts.UI
{
    public class InGameStartFade : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;
        
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.DOFade(0, 0.7f).OnComplete(() => fadeImage.gameObject.SetActive(false));
        }
    }
}