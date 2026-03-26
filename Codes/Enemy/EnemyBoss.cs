    using System;
    using System.Collections;
    using _01_Scripts.CameraScript;
    using _01_Scripts.Combat;
    using _01_Scripts.Core;
    using _01_Scripts.Entities;
    using _01_Scripts.Players;
    using Blade.Enemies.BT.Events;
    using UnityEngine;
    using UnityEngine.Events;

    namespace _01_Scripts.Enemy
    {
        public class EnemyBoss : Enemy
        {
            public UnityEvent<Vector3, float> OnKnockBackInvoke;
            private StateChange _stateChannel;
            private EntityAnimatorTrigger _animatorTrigger;
            private EntityHealth _healthCompo;
            private Collider _collider;
            
            public float meleeRange = 2f;
            public float jumpRange = 3f;

            [SerializeField] private GameObject radius;

            [SerializeField] private float meleeDamage;
            [SerializeField] private float jumpDamage;
            
            protected override void Awake()
            {
                base.Awake();
                _collider = GetComponent<Collider>();
                _healthCompo = GetComponent<EntityHealth>();
            }

            protected override void Start()
            {
                base.Start();
                _animatorTrigger = GetComponentInChildren<EntityAnimatorTrigger>();

                _animatorTrigger.OnBossSwingAttackTrigger += CheckSwingAttackRadius;
                _animatorTrigger.OnBossJumpAttackTrigger += CheckJumpAttackRadius;
                
                _animatorTrigger.OnSwingDamageCastTrigger += HandleSpinAttackEvent;
                _animatorTrigger.OnJumpingDamageCastTrigger += HandleJumpingAttackEvent;
                GameManager.Instance.OnGameOver += Initialize;
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
                GameManager.Instance.AddEnemyCount(4);
                yield return new WaitForSeconds(3f);
                _pool.Push(this);
                _healthCompo.currentHealth = _healthCompo.maxHealth;
                IsDead = false;
                _collider.enabled = true;
                _stateChannel.SendEventMessage(EnemyState.CHASE);
            }

            private void CheckJumpAttackRadius()
            {
                radius.SetActive(true);
                radius.transform.localScale = new Vector3(jumpRange * 2, 0.1f, jumpRange * 2);
            }
            
            private void CheckSwingAttackRadius()
            {
                radius.SetActive(true);
                radius.transform.localScale = new Vector3(meleeRange * 2, 0.1f, meleeRange * 2);
            }

            private void HandleJumpingAttackEvent()
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, jumpRange);
                foreach (var collider in hitColliders)
                {
                    if (collider.TryGetComponent(out Player player))
                    {
                        if (collider.TryGetComponent(out EntityHealth entityHealth))
                        {
                            entityHealth.ApplyDamage(jumpDamage);
                            CameraShake.Instance.Shake();
                        }
                    }
                }
                radius.SetActive(false);
            }
            
            private void HandleSpinAttackEvent()
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, meleeRange);
                foreach (var collider in hitColliders)
                {
                    if (collider.TryGetComponent(out Player player))
                    {
                        if (collider.TryGetComponent(out EntityHealth entityHealth))
                        {
                            entityHealth.ApplyDamage(meleeDamage);
                            CameraShake.Instance.Shake();
                        }
                    }
                }
                radius.SetActive(false);
            }

            private void OnDrawGizmos()
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, meleeRange);

                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, jumpRange);
            }
        }
    }