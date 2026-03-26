using System;
using _01_Scripts.Combat;
using _01_Scripts.Entities;
using KJYLib.StatSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _01_Scripts.Players
{
    public class PlayerStat : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        public StatSO atkPowerStat, critPowerStat, critPerStat, atkSpeedStat;

        public Action OnStatValueChanged;

        private float _damage;
        private float _critPer;
        private float _critPower;
        private float _attackSpeed;
        
        public float Damage
        {
            get => _damage;
            set
            {
                if (_damage != value)
                {
                    _damage = value;
                    OnStatValueChanged?.Invoke();
                }
            }
        }
        public float CritPer
        {
            get => _critPer;
            set
            {
                if (_critPer != value)
                {
                    _critPer = value;
                    OnStatValueChanged?.Invoke();
                }
            }
        }
        public float CritPower
        {
            get => _critPower;
            set
            {
                if (_critPower != value)
                {
                    _critPower = value;
                    OnStatValueChanged?.Invoke();
                }
            }
        }
        public float AttackSpeed
        {
            get => _attackSpeed;
            set
            {
                if (_attackSpeed != value)
                {
                    _attackSpeed = value;
                    OnStatValueChanged?.Invoke();
                }
            }
        }
        
        private EntityStat _statCompo;
        public EntityHealth _healthCompo { get; set; }

        public void Initialize(Entity entity)
        {
            _statCompo = entity.GetCompo<EntityStat>();
            _healthCompo = entity.GetCompo<EntityHealth>();
        }

        private void Start()
        {
            OnStatValueChanged?.Invoke();
        }

        public void AfterInitialize()
        {
            Damage = _statCompo.SubscribeStat(atkPowerStat, HandleDamage, 5);
            CritPer = _statCompo.SubscribeStat(critPerStat, HandleCritPer, 0);
            CritPower = _statCompo.SubscribeStat(critPowerStat, HandleCritPower, 1);
            AttackSpeed = _statCompo.SubscribeStat(atkSpeedStat, HandleAtkSpeed, 1);
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(atkPowerStat, HandleDamage);
            _statCompo.UnSubscribeStat(critPerStat, HandleCritPer);
            _statCompo.UnSubscribeStat(critPowerStat, HandleCritPower);
            _statCompo.UnSubscribeStat(atkSpeedStat, HandleAtkSpeed);
        }

        private void HandleCritPer(StatSO stat, float currentValue, float prevValue)
        {
            CritPer = prevValue;
        }

        private void HandleCritPower(StatSO stat, float currentValue, float prevValue)
        {
            CritPower = prevValue;
        }

        private void HandleDamage(StatSO stat, float currentValue, float prevValue)
        {
            Damage = prevValue;
        }
        
        private void HandleAtkSpeed(StatSO stat, float currentValue, float prevValue)
        {
            AttackSpeed = prevValue;
        }
        
    }
}