using _01_Scripts.CameraScript;
using _01_Scripts.Entities;
using KJYLib.StatSystem;
using UnityEngine;

namespace _01_Scripts.Combat
{
    public class EntityHealth : MonoBehaviour,IEntityComponent, IAfterInitialize
    {
        private Entity _entity;
        private EntityStat _statCompo;

        [SerializeField] private StatSO hpStat;
        public float maxHealth;
        public float currentHealth;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = entity.GetCompo<EntityStat>();
        }

        public void AddCurrentHealth(float amount)
        {
            currentHealth += amount;
            if (currentHealth >= maxHealth)
                currentHealth = maxHealth;
        }

        public void AddMaxHealth(float amount)
        {
            maxHealth += amount;
        }

        public void AfterInitialize()
        {
            currentHealth = maxHealth = _statCompo.SubscribeStat(hpStat, HandleMaxHpChange, 10f);
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(hpStat, HandleMaxHpChange);
        }

        private void HandleMaxHpChange(StatSO stat, float currentValue, float prevValue)
        {
            float changed = currentValue - prevValue;
            maxHealth = currentValue;
            if (changed > 0)
            {
                currentHealth = Mathf.Clamp(currentHealth + changed, 0, maxHealth);
            }
            else
            {
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            }
        }

        public void ApplyDamage(float damage)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
            if (currentHealth <= 0)
            {
                _entity.OnDeadEvent?.Invoke();
            }

            _entity.OnHitEvent?.Invoke();
        }

        
        
    }
}