using System.Collections.Generic;
using _01_Scripts.Combat;
using _01_Scripts.Enemy;
using EnemyEntity = _01_Scripts.Enemy.Enemy;
using KJYLib.ObjectPool.RunTime;
using UnityEngine;

namespace _01_Scripts.Players.Bullet
{
    public class BulletMove : MonoBehaviour, IPoolable
    {
        [SerializeField] private LayerMask _layer;
        [SerializeField] private float speed = 15f;
        [field: SerializeField] public PoolItemSO PoolItem { get; set; }
        public GameObject GameObject => gameObject;
        public float Damage { get; set; }
        private Pool _pool;
        private float _life;
        private int _piercing, _burnLevel;
        private int _ricochets, _frostLevel;
        private bool _redirected;
        private bool _returned;
        private TrailRenderer _trail;
        private Color _color;
        private readonly HashSet<EnemyEntity> _hit = new HashSet<EnemyEntity>();
        private readonly List<Collider> _ignored = new List<Collider>();
        private Collider _collider;

        public void Configure(int piercing, int burnLevel, bool drone = false, int ricochets = 0, int frostLevel = 0)
        {
            _piercing = piercing;
            _burnLevel = burnLevel;
            _ricochets = ricochets;
            _frostLevel = frostLevel;
            _color = drone ? new Color(0.7f, 0.3f, 1f) : frostLevel > 0 ? PlayerItems.ItemColor(ItemType.Frost) : burnLevel > 0 ?
                new Color(1f, 0.3f, 0.04f) : piercing > 0 ? Color.cyan : new Color(1f, 0.9f, 0.4f);
            if (!drone && frostLevel == 0 && burnLevel == 0 && ricochets > 0)
                _color = PlayerItems.ItemColor(ItemType.Ricochet);
            if (_trail == null)
            {
                _trail = GetComponentInChildren<TrailRenderer>();
                if (_trail == null) _trail = gameObject.AddComponent<TrailRenderer>();
                _trail.sharedMaterial = ItemVisual.Material;
                _trail.minVertexDistance = 0.05f;
                _trail.endWidth = 0;
            }
            _trail.Clear();
            _trail.startWidth = burnLevel > 0 ? 0.22f : 0.12f;
            _trail.time = piercing > 0 ? 0.3f : 0.15f;
            _trail.startColor = _color;
            Color endColor = burnLevel > 0 ? PlayerItems.ItemColor(ItemType.Incendiary) : _color;
            _trail.endColor = new Color(endColor.r, endColor.g, endColor.b, 0);
        }

        private void FixedUpdate()
        {
            if (_returned) return;
            _redirected = false;
            _life += Time.fixedDeltaTime;
            if (_life >= 3f) { ReturnToPool(); return; }
            float distance = speed * Time.fixedDeltaTime;
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 0.12f, transform.forward,
                distance, _layer.value | LayerMask.GetMask("Obstarcle"), QueryTriggerInteraction.Ignore);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.GetComponentInParent<EnemyEntity>() == null)
                {
                    ReturnToPool();
                    return;
                }
                HitEnemy(hit.collider, hit.point);
                if (_returned || _redirected) return;
            }
            transform.position += transform.forward * distance;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_returned || _redirected) return;
            EnemyEntity enemy = collision.collider.GetComponentInParent<EnemyEntity>();
            if (enemy != null)
                HitEnemy(collision.collider, collision.contactCount > 0 ? collision.GetContact(0).point : transform.position);
            else if (collision.collider.GetComponentInParent<Player>() == null &&
                     collision.collider.GetComponentInParent<BulletMove>() == null)
                ReturnToPool();
        }

        private void HitEnemy(Collider collider, Vector3 point)
        {
            EnemyEntity enemy = collider.GetComponentInParent<EnemyEntity>();
            if (enemy == null || enemy.IsDead || !_hit.Add(enemy)) return;
            EntityHealth health = enemy.GetComponentInChildren<EntityHealth>();
            if (health == null) return;
            enemy.GetComponentInChildren<EnemyHitText>()?.DamageText(Damage);
            health.ApplyDamage(Damage);
            ItemVisual.Burst(point, _color, _burnLevel > 0 ? 0.7f : 0.3f);
            if (_frostLevel > 0)
            {
                ItemVisual.FrostBurst(point);
                if (!enemy.IsDead)
                {
                    ItemFrost frost = enemy.GetComponent<ItemFrost>();
                    if (frost == null) frost = enemy.gameObject.AddComponent<ItemFrost>();
                    frost.enabled = true;
                    frost.Apply(enemy, _frostLevel);
                }
            }
            if (_burnLevel > 0 && !enemy.IsDead)
            {
                ItemBurn burn = enemy.GetComponent<ItemBurn>();
                if (burn == null) burn = enemy.gameObject.AddComponent<ItemBurn>();
                burn.enabled = true;
                burn.Apply(enemy, Damage * (0.15f + _burnLevel * 0.1f), 2 + _burnLevel);
            }
            if (_ricochets > 0 && TryRicochet(enemy, point))
            {
                IgnoreEnemy(enemy);
                return;
            }
            if (_piercing-- <= 0) { ReturnToPool(); return; }
            IgnoreEnemy(enemy);
        }

        private void IgnoreEnemy(EnemyEntity enemy)
        {
            foreach (Collider part in enemy.GetComponentsInChildren<Collider>())
            {
                if (_collider == null) continue;
                Physics.IgnoreCollision(_collider, part, true);
                _ignored.Add(part);
            }
        }

        private bool TryRicochet(EnemyEntity source, Vector3 point)
        {
            Vector3 origin = source.GetComponentInChildren<EntityHealth>().transform.position + Vector3.up;
            Collider nearest = null;
            float distance = 64f;
            foreach (Collider candidate in Physics.OverlapSphere(origin, 8f, _layer, QueryTriggerInteraction.Ignore))
            {
                EnemyEntity target = candidate.GetComponentInParent<EnemyEntity>();
                if (target == null || target.IsDead || !target.isActiveAndEnabled || _hit.Contains(target)) continue;
                Vector3 destination = candidate.bounds.center;
                float sqr = (destination - origin).sqrMagnitude;
                if (sqr <= 0.01f || sqr >= distance || Physics.Linecast(origin, destination, LayerMask.GetMask("Obstarcle"))) continue;
                distance = sqr;
                nearest = candidate;
            }
            if (nearest == null) return false;
            Vector3 direction = (nearest.bounds.center - origin).normalized;
            transform.SetPositionAndRotation(origin + direction * 0.2f, Quaternion.LookRotation(direction));
            ItemVisual.RicochetLink(point, nearest.bounds.center);
            _ricochets--;
            Damage *= 0.8f;
            _redirected = true;
            return true;
        }

        private void ReturnToPool()
        {
            if (_returned) return;
            _returned = true;
            _pool.Push(this);
        }

        private void OnDisable()
        {
            foreach (Collider collider in _ignored)
                if (collider != null && _collider != null) Physics.IgnoreCollision(_collider, collider, false);
            _ignored.Clear();
            if (_trail != null) _trail.Clear();
        }

        public void SetUpPool(Pool pool) => _pool = pool;
        public void ResetItem()
        {
            _life = 0;
            _returned = false;
            _piercing = _burnLevel = 0;
            _ricochets = _frostLevel = 0;
            _redirected = false;
            Damage = 0;
            _hit.Clear();
            _collider = GetComponent<Collider>();
            if (TryGetComponent(out Rigidbody body))
            {
                body.linearVelocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }
            if (_trail != null) _trail.Clear();
        }
    }
}
