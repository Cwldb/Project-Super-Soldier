using System;
using System.Collections;
using _01_Scripts.CameraScript;
using _01_Scripts.Combat;
using _01_Scripts.Effect;
using _01_Scripts.Entities;
using _01_Scripts.Shelling;
using _01_Work.JY._01_Scripts.Manager;
using KJYLib.Dependencies;
using KJYLib.ObjectPool.RunTime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _01_Scripts.Players
{
    public class PlayerSkillSet : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private LayerMask layer;
        [Header("Active1")]
        [SerializeField] private PoolItemSO smokeParticle;
        [SerializeField] private PoolItemSO crackParticle;
        public float radius = 4;
        public float damage = 50;
        public float cooldownTime1 = 10f;
        
        [Header("Active2")]
        [SerializeField] private PoolItemSO bombPrefab;
        public float bombSpawnRange = 7f;
        public int bombCount = 30;
        public float cooldownTime2 = 30f;

        [Inject] private PoolManagerMono _poolManager;
        
        private Player _player;
        private Collider[] _targets = new Collider[100];
        public float CurrentCooldown1 { get; set; } = 6f;
        public float CurrentCooldown2 { get; set; } = 6f;

        public bool CanUseActive1 { get; set; } = true;
        public bool CanUseActive2 { get; set; } = true;
        
        public void Initialize(Entity entity)
        {
            _player = entity as Player;
        }

        private void Start()
        {
            CurrentCooldown1 = cooldownTime1;
            CurrentCooldown2 = cooldownTime2;
        }

        private void Update()
        {
            if (!CanUseActive1)
            {
                CurrentCooldown1 += Time.deltaTime;
                if (CurrentCooldown1 >= cooldownTime1)
                    CanUseActive1 = true;
            }

            if (!CanUseActive2)
            {
                CurrentCooldown2 += Time.deltaTime;
                if (CurrentCooldown2 >= cooldownTime2)
                    CanUseActive2 = true;
            }
        }

        public void UseActive1()
        {
            CanUseActive1 = false;
            CurrentCooldown1 = 0;
            int hitCount = Physics.OverlapSphereNonAlloc(transform.position, radius, _targets, layer);
            for (int i = 0; i < hitCount; i++)
            {
                Collider target = _targets[i];
                if (target.TryGetComponent(out EntityHealth health))
                    health.ApplyDamage(damage);
            }
            PlayEffect();
            CameraShake.Instance.Active1Shake();
        }

        private async void PlayEffect()
        {
            AudioManager.Instance.PlaySfx("ACTIVE1");
            PoolingEffect smokeEffect = _poolManager.Pop<PoolingEffect>(smokeParticle);
            PoolingEffect crackEffect = _poolManager.Pop<PoolingEffect>(crackParticle);
            smokeEffect.PlayerVFX(transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
            crackEffect.PlayerVFX(transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
            
            await Awaitable.WaitForSecondsAsync(1f);
            _poolManager.Push(smokeEffect);
            _poolManager.Push(crackEffect);
        }
        
        public void UseActive2()
        {
            CanUseActive2 = false;
            CurrentCooldown2 = 0;
            for (int i = 0; i < bombCount; i++)
            {
                StartCoroutine(SpawnBomb());
            }
        }

        private IEnumerator SpawnBomb()
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
            Vector3 randomPos = GetRandomPositionAroundPlayer();
            ExplosionBomb bomb = _poolManager.Pop<ExplosionBomb>(bombPrefab);
            bomb.poolManager = _poolManager;
            bomb.transform.position = randomPos;
        }

        private Vector3 GetRandomPositionAroundPlayer()
        {
            Vector2 circle = UnityEngine.Random.insideUnitCircle * bombSpawnRange;
            Vector3 randomPos = _player.transform.position + new Vector3(circle.x, 20f, circle.y);
            return randomPos;
        }
    }
}