using System;
using System.Collections;
using _01_Scripts.CameraScript;
using _01_Scripts.Combat;
using _01_Scripts.Core;
using _01_Scripts.Entities;
using _01_Scripts.Players;
using Blade.Enemies.BT.Events;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace _01_Scripts.Enemy
{
    public class EnemySoldiers : Enemy
    {
        public UnityEvent<Vector3, float> OnKnockBackInvoke;
        private StateChange _stateChannel;
        private EntityAnimatorTrigger _animatorTrigger;
        private EnemyAttackCompo _attackCompo;
        private EntityHealth _healthCompo;
        private Collider _collider;
        private NavMeshAgent _agent;

        protected override void Awake()
        {
            base.Awake();
            _collider = GetComponent<Collider>();
            _healthCompo = GetComponent<EntityHealth>();
            _agent = GetComponentInChildren<NavMeshAgent>();
        }

        protected override void Start()
        {
            base.Start();
            _attackCompo = GetComponentInChildren<EnemyAttackCompo>();
            _animatorTrigger = GetComponentInChildren<EntityAnimatorTrigger>();
            GameManager.Instance.OnGameOver += Initialize;
            
            _animatorTrigger.OnDamageCastTrigger += HandleAttackEvent;
            OnDeadEvent.AddListener(HandleDeathEvent);
            _stateChannel = GetBlackboardVariable<StateChange>("StateChannel").Value;
        }

        private void Initialize()
        {
            _pool.Push(this);
        }

        private void HandleDeathEvent()
        {
            if (IsDead) return;
            IsDead = true;
            _stateChannel.SendEventMessage(EnemyState.DEAD);
            StartCoroutine(EnemyDying());
        }

        private IEnumerator EnemyDying()
        {
            yield return new WaitForSeconds(1);
            GameManager.Instance.AddEnemyCount();
            yield return new WaitForSeconds(3f);
            _pool.Push(this);
            _healthCompo.currentHealth = _healthCompo.maxHealth;
            IsDead = false;
            _collider.enabled = true;
            _agent.enabled = true;
            _stateChannel.SendEventMessage(EnemyState.CHASE);
        }

        private void HandleAttackEvent()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
            foreach (var collider in hitColliders)
            {
                if (collider.TryGetComponent(out Player player))
                {
                    if (collider.TryGetComponent(out EntityHealth entityHealth))
                    {
                        entityHealth.ApplyDamage(_attackCompo.Damage);
                        CameraShake.Instance.Shake();
                    }
                }
            }
        }
    }
}