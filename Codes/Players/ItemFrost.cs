using _01_Scripts.Enemy;
using UnityEngine;
using EnemyEntity = _01_Scripts.Enemy.Enemy;

namespace _01_Scripts.Players
{
    public class ItemFrost : MonoBehaviour
    {
        private EnemyEntity _enemy;
        private NavMovement _movement;
        private float _remaining;
        private int _level;

        public void Apply(EnemyEntity enemy, int level)
        {
            _enemy = enemy;
            _movement = enemy.GetComponentInChildren<NavMovement>();
            _level = Mathf.Max(_level, level);
            _remaining = Mathf.Max(_remaining, 1.5f + level * 0.5f);
            if (_movement != null) _movement.SetSpeedMultiplier(1f - (0.2f + _level * 0.1f));
        }

        private void Update()
        {
            if (Time.timeScale == 0) return;
            _remaining -= Time.deltaTime;
            if (_remaining <= 0 || _enemy == null || _enemy.IsDead) enabled = false;
        }

        private void OnDisable()
        {
            if (_movement != null) _movement.SetSpeedMultiplier(1f);
            _remaining = 0;
            _level = 0;
        }
    }
}
