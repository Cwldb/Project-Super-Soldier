using System.Collections.Generic;
using _01_Scripts.Combat;
using _01_Scripts.Core;
using EnemyEntity = _01_Scripts.Enemy.Enemy;
using UnityEngine;

namespace _01_Scripts.Players
{
    public class ItemMine : MonoBehaviour
    {
        private int _level;
        private float _damage, _age;
        public static void Spawn(Vector3 position, int level, float damage)
        {
            GameObject obj = ItemVisual.CreateOrb("Tactical Mine", position, new Color(1f, 0.7f, 0.1f), 0.5f);
            obj.transform.localScale = new Vector3(0.5f, 0.15f, 0.5f);
            ItemMine mine = obj.AddComponent<ItemMine>();
            mine._level = level;
            mine._damage = damage * (1.5f + level * 0.5f);
        }

        private void Update()
        {
            if (Time.timeScale == 0) return;
            if (TimerManager.Instance != null && TimerManager.Instance.isDeath) { Destroy(gameObject); return; }
            _age += Time.deltaTime;
            if (_age > 15) { Destroy(gameObject); return; }
            if (_age < 0.6f) return;
            foreach (Collider hit in Physics.OverlapSphere(transform.position, 1.2f))
            {
                EnemyEntity enemy = hit.GetComponentInParent<EnemyEntity>();
                if (enemy == null || enemy.IsDead) continue;
                Explode();
                break;
            }
        }

        private void Explode()
        {
            float radius = 2f + _level * 0.6f;
            var damaged = new HashSet<EnemyEntity>();
            foreach (Collider hit in Physics.OverlapSphere(transform.position, radius))
            {
                EnemyEntity enemy = hit.GetComponentInParent<EnemyEntity>();
                if (enemy == null || enemy.IsDead || !damaged.Add(enemy)) continue;
                enemy.GetComponentInChildren<EntityHealth>()?.ApplyDamage(_damage);
            }
            ItemVisual.Burst(transform.position, new Color(1f, 0.65f, 0.1f), radius);
            Destroy(gameObject);
        }
    }
}

