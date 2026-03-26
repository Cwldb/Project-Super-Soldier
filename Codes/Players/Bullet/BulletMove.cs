using System.Collections;
using _01_Scripts.Combat;
using _01_Scripts.Effect;
using _01_Scripts.Enemy;
using KJYLib.ObjectPool.RunTime;
using Unity.VisualScripting;
using UnityEngine;
using IPoolable = KJYLib.ObjectPool.RunTime.IPoolable;

namespace _01_Scripts.Players.Bullet
{
    public class BulletMove : MonoBehaviour, IPoolable
    {
        [SerializeField] private LayerMask _layer;
        [SerializeField] private float speed = 15f;
        [field : SerializeField] public PoolItemSO PoolItem { get; set; }
        public GameObject GameObject => gameObject;

        public float Damage { get; set; }
        
        private Pool _pool;

        private void Start()
        {
            StartCoroutine(BulletLifeTime());
        }

        private IEnumerator BulletLifeTime()
        {
            yield return new WaitForSeconds(3f);
            _pool.Push(this);
        }

        private void FixedUpdate()
        {
            transform.position += transform.forward * (speed * Time.fixedDeltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out EntityHealth health) || collision.gameObject.layer == _layer)
            {
                var contact = collision.contacts[0];
                EnemyHitText text = collision.gameObject.transform.GetComponentInChildren<EnemyHitText>();
                if (text != null)
                    text.DamageText(Damage);
                health.ApplyDamage(Damage);
                _pool.Push(this);
            }
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