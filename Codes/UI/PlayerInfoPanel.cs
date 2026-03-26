using System;
using _01_Scripts.Combat;
using _01_Scripts.Core;
using _01_Scripts.Entities;
using _01_Scripts.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Scripts.UI
{
    public class PlayerInfoPanel : MonoBehaviour
    {
        [Header("Texts")]
        [SerializeField] private TMP_Text atkText;
        [SerializeField] private TMP_Text aspdText;
        [SerializeField] private TMP_Text criText;
        [SerializeField] private TMP_Text dcriText;
        
        [Header("Health")]
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Image healthBar;
        
        [Header("Skills")]
        [SerializeField] private Image skill1Image;
        [SerializeField] private Image skill2Image;
        
        private PlayerStat _playerStat;
        private PlayerSkillSet _playerSkillSet;
        private EntityHealth _playerHealth;
        private Entity _entity;

        private void Start()
        {
            _playerStat = GameManager.Instance.PlayerFinder.Target.GetCompo<PlayerStat>();
            _playerSkillSet = GameManager.Instance.PlayerFinder.Target.GetCompo<PlayerSkillSet>();
            _playerHealth = GameManager.Instance.PlayerFinder.Target.GetCompo<EntityHealth>();
            _entity = GameManager.Instance.PlayerFinder.Target;
            
            _playerStat.OnStatValueChanged += UpdateStat; 
            _entity.OnHitEvent.AddListener(UpdateHealth);
            GameManager.Instance.OnValueChange += UpdateHealth;
            _playerStat.OnStatValueChanged.Invoke();
            skill1Image.fillAmount = 1;
        }

        private void Update()
        {
            if (_playerSkillSet != null)
            {
                if(!_playerSkillSet.CanUseActive1)
                    skill1Image.fillAmount = _playerSkillSet.CurrentCooldown1 / _playerSkillSet.cooldownTime1;
                if(!_playerSkillSet.CanUseActive2)
                    skill2Image.fillAmount = _playerSkillSet.CurrentCooldown2 / _playerSkillSet.cooldownTime2;
            }
        }

        private void UpdateStat()
        {
            atkText.text = $"{Mathf.Floor(_playerStat.Damage * 10f) / 10f}";
            aspdText.text = $"{_playerStat.AttackSpeed}초";
            criText.text = $"{_playerStat.CritPer * 100}%";
            dcriText.text = $"{_playerStat.CritPower}배";
        }

        private void UpdateHealth()
        {
            healthText.text = $"{_playerHealth.currentHealth} / {_playerHealth.maxHealth}";
            healthBar.fillAmount = _playerHealth.currentHealth / _playerHealth.maxHealth;
        }
    }
}
