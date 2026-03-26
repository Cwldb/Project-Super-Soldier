using _01_Scripts.Entities;
using KJYLib.StatSystem;
using UnityEngine;

namespace _01_Scripts.Enemy
{
    public class EnemyAttackCompo : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [SerializeField] private StatSO damageStat;

        private EntityStat _statCompo;

        public float Damage { get; set; }
    
        public void Initialize(Entity entity)
        {
            _statCompo = entity.GetCompo<EntityStat>();
        }

        public void AfterInitialize()
        {
            Damage = _statCompo.SubscribeStat(damageStat, HandleDamage, 3);
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(damageStat, HandleDamage);
        }

        private void HandleDamage(StatSO stat, float currentValue, float prevValue)
        {
            Damage = prevValue;
        }
    }
}
