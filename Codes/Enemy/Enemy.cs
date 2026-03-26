using _01_Scripts.Entities;
using KJYLib.Dependencies;
using KJYLib.ObjectPool.RunTime;
using Unity.Behavior;
using UnityEngine;

namespace _01_Scripts.Enemy
{
    public abstract class Enemy : Entity, IPoolable
    {
        [field : SerializeField] public EntityFinderSO PlayerFinder { get; set; }
        [field : SerializeField] public PoolItemSO PoolItem { get; set; }
        public GameObject GameObject => gameObject;
        public BehaviorGraphAgent BtAgent { get; private set; }
        protected Animator _animator;

        protected Pool _pool;

        #region temp
        public float attackRange = 2f;
        #endregion

        protected override void AddComponents()
        {
            base.AddComponents();
            _animator = GetComponentInChildren<Animator>();
            BtAgent = GetComponent<BehaviorGraphAgent>();
            Debug.Assert(BtAgent != null, $"{gameObject.name} don't have BehaviorGraphAgent");
        }

        public BlackboardVariable<T> GetBlackboardVariable<T>(string key)
        {
            if(BtAgent.GetVariable(key, out BlackboardVariable<T> result))
                return result;
            return default;
        }

        public void SetUpPool(Pool pool)
        {
            _pool = pool;
        }

        public virtual void ResetItem()
        {
            
        }
    }
}