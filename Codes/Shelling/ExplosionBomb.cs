using _01_Scripts.CameraScript;
using _01_Scripts.Combat;
using _01_Scripts.Effect;
using _01_Work.JY._01_Scripts.Manager;
using Blade.Effects;
using KJYLib.Dependencies;
using KJYLib.ObjectPool.RunTime;
using UnityEngine;

namespace _01_Scripts.Shelling
{
    public class ExplosionBomb : MonoBehaviour, IPoolable
    {
        [field : SerializeField] public PoolItemSO PoolItem { get; set; }
        [SerializeField] private LayerMask layer;
        [SerializeField] private float radius;
        [SerializeField] private float damage;
        
        [SerializeField] private PoolItemSO particle;

        public PoolManagerMono poolManager { get; set; }
        
        public GameObject GameObject => gameObject;
        private readonly Collider[] _targets = new Collider[100];
        private Pool _pool;
        
        private void OnCollisionEnter(Collision collision)
        {
            Explode();
        }

        private void Explode()
        {
            AudioManager.Instance.PlaySfx("ACTIVE2");
            int hitCount = Physics.OverlapSphereNonAlloc(transform.position, radius, _targets, layer);
            for (int i = 0; i < hitCount; i++)
            {
                Collider target = _targets[i];
                if(target.TryGetComponent(out EntityHealth health))
                    health.ApplyDamage(damage);
            }
            ExplodeEffect();
            _pool.Push(this);
        }

        private async void ExplodeEffect()
        {
            PoolingEffect effect = poolManager.Pop<PoolingEffect>(particle);
            effect.PlayerVFX(transform.position, Quaternion.identity);
            CameraShake.Instance.Active2Shake();

            await Awaitable.WaitForSecondsAsync(1f);
            poolManager.Push(effect);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public void SetUpPool(Pool pool)
        {
            _pool = pool;
        }

        public void ResetItem()
        {
            
        }
    }
}