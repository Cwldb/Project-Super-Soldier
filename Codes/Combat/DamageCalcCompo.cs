using _01_Scripts.Entities;
using KJYLib.StatSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _01_Scripts.Combat
{
    public class DamageCalcCompo : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [SerializeField] private StatSO criticalStat, criticalDamageStat;

        private EntityStat _statCompo;
        private float _critical, _criticalDamage;

        public void Initialize(Entity entity)
        {
            _statCompo = entity.GetComponentInChildren<EntityStat>();
            if (_statCompo == null)
                Debug.Log("dd");
        }

        public void AfterInitialize()
        {
            if (criticalStat is null)
                _critical = 0;
            else
            {
                _critical = _statCompo.SubscribeStat(criticalStat, HandleCriticalChange, 0f);
            }

            if (criticalDamageStat is null)
                _criticalDamage = 1;
            else
            {
                _criticalDamage = _statCompo.SubscribeStat(criticalDamageStat, HandleCriticalDamageChange, 1f);
            }
        }

        private void OnDestroy()
        {
            if (criticalStat is not null)
                _statCompo.UnSubscribeStat(criticalStat, HandleCriticalChange);
            if (criticalDamageStat is not null)
                _statCompo.UnSubscribeStat(criticalDamageStat, HandleCriticalDamageChange);
        }

        private void HandleCriticalChange(StatSO stat, float currentValue, float prevValue) => _critical = currentValue;
        private void HandleCriticalDamageChange(StatSO stat, float currentValue, float prevValue) => _criticalDamage = currentValue;
    }
}
