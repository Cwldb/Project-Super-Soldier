using _01_Scripts.Combat;
using EnemyEntity = _01_Scripts.Enemy.Enemy;
using UnityEngine;

namespace _01_Scripts.Players
{
    public class ItemBurn : MonoBehaviour
    {
        private EnemyEntity _enemy;
        private EntityHealth _health;
        private float _remaining, _damage, _tick;

        public void Apply(EnemyEntity enemy, float damage, float duration)
        {
            _enemy = enemy;
            _health = enemy.GetComponentInChildren<EntityHealth>();
            _remaining = Mathf.Max(_remaining, duration);
            _damage = Mathf.Max(_damage, damage);
        }

        private void Update()
        {
            if (Time.timeScale == 0) return;
            if (_enemy == null || _enemy.IsDead || _health == null) { enabled = false; return; }
            _remaining -= Time.deltaTime;
            _tick -= Time.deltaTime;
            if (_remaining <= 0) { enabled = false; return; }
            if (_tick > 0) return;
            _tick = 0.5f;
            ItemVisual.Burst(transform.position + Vector3.up, new Color(1f, 0.3f, 0.05f), 0.25f);
            _health.ApplyDamage(_damage * 0.5f);
        }

        private void OnDisable()
        {
            _remaining = _damage = _tick = 0;
        }
    }
}

