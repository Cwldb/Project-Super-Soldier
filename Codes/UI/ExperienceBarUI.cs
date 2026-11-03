using _01_Scripts.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Scripts.UI
{
    public class ExperienceBarUI : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private TMP_Text experienceText;
        private GameManager _manager;

        private void Start()
        {
            _manager = GameManager.Instance;
            _manager.OnExperienceChanged += Refresh;
            Refresh();
        }

        private void OnEnable()
        {
            if (_manager == null) return;
            _manager.OnExperienceChanged += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            if (_manager != null) _manager.OnExperienceChanged -= Refresh;
        }

        private void Refresh()
        {
            fillImage.fillAmount = _manager.ExperienceProgress;
            experienceText.text = $"Lv.{_manager.GetCurLevel()}    {_manager.CurrentExperience} / {_manager.RequiredExperience} XP";
        }

    }
}
