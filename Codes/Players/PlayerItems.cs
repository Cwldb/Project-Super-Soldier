using System.Collections.Generic;
using _01_Scripts.Core;
using EnemyEntity = _01_Scripts.Enemy.Enemy;
using UnityEngine;

namespace _01_Scripts.Players
{
    public enum ItemType { Piercing, Incendiary, Drone, Mine, Ricochet, Frost }

    // Kept separate from stats and skills so existing prefabs and state machines still work.
    public class PlayerItems : MonoBehaviour
    {
        public const int MaxLevel = 3;
        public event System.Action OnItemsChanged;
        private readonly int[] _levels = new int[System.Enum.GetValues(typeof(ItemType)).Length];
        private readonly List<Transform> _drones = new List<Transform>();
        private Player _player;
        private PlayerStat _stats;
        private PlayerFire _fire;
        private float _droneTimer, _mineTimer;
        private Vector3 _lastMinePosition;

        public int GetLevel(ItemType item) => _levels[(int)item];
        public bool CanUpgrade(ItemType item) => GetLevel(item) < MaxLevel;
        public void Upgrade(ItemType item)
        {
            if (!CanUpgrade(item)) return;
            _levels[(int)item]++;
            OnItemsChanged?.Invoke();
        }

        public static string ItemName(ItemType item)
        {
            switch (item)
            {
                case ItemType.Piercing: return "관통탄";
                case ItemType.Incendiary: return "소이탄";
                case ItemType.Drone: return "호위 드론";
                case ItemType.Ricochet: return "도탄 탄창";
                case ItemType.Frost: return "냉각탄";
                default: return "전술 지뢰";
            }
        }

        public static Color ItemColor(ItemType item)
        {
            switch (item)
            {
                case ItemType.Piercing: return Color.cyan;
                case ItemType.Incendiary: return new Color(1f, 0.4f, 0.1f);
                case ItemType.Drone: return new Color(0.7f, 0.4f, 1f);
                case ItemType.Ricochet: return new Color(1f, 0.85f, 0.15f);
                case ItemType.Frost: return new Color(0.35f, 0.65f, 1f);
                default: return Color.yellow;
            }
        }

        public string Description(ItemType item, bool nextLevel = true)
        {
            int next = Mathf.Clamp(GetLevel(item) + (nextLevel ? 1 : 0), 1, MaxLevel);
            switch (item)
            {
                case ItemType.Piercing: return $"총알이 적 {next}명을 추가 관통합니다.\n청록색 관통 궤적";
                case ItemType.Incendiary: return $"명중 시 {2 + next}초 동안 화상 피해를 줍니다.\n주황색 불꽃 궤적";
                case ItemType.Drone: return $"드론 {next}기가 주변 적을 자동 공격합니다.\n보라색 에너지탄";
                case ItemType.Ricochet: return $"명중 후 주변 적에게 최대 {next}회 튕깁니다. 도탄할 때마다 피해가 20% 감소합니다.\n노란 연결 궤적";
                case ItemType.Frost: return $"명중한 적의 이동 속도를 {20 + next * 10}% 감소시킵니다. 지속 시간 {1.5f + next * 0.5f:0.0}초.\n푸른 냉기 궤적과 얼음 파편";
                default: return $"이동 경로에 {5 - next * 0.7f:0.0}초마다 지뢰를 설치합니다.\n강화할수록 폭발 범위와 피해 증가";
            }
        }

        private void Start()
        {
            _player = GetComponent<Player>();
            _stats = _player.GetCompo<PlayerStat>();
            _fire = _player.GetCompo<PlayerFire>();
            _lastMinePosition = transform.position;
        }

        private void Update()
        {
            if (_player == null || _player.IsDead || Time.timeScale == 0 ||
                (TimerManager.Instance != null && TimerManager.Instance.isDeath)) return;

            int droneLevel = GetLevel(ItemType.Drone);
            while (_drones.Count < droneLevel)
            {
                var drone = ItemVisual.CreateOrb("Escort Drone", transform.position, new Color(0.7f, 0.3f, 1f), 0.3f);
                drone.transform.SetParent(transform);
                _drones.Add(drone.transform);
            }
            for (int i = 0; i < _drones.Count; i++)
            {
                float angle = Time.time * 1.5f + i * Mathf.PI * 2 / _drones.Count;
                _drones[i].position = transform.position + new Vector3(Mathf.Cos(angle) * 1.4f, 1.8f, Mathf.Sin(angle) * 1.4f);
            }
            _droneTimer -= Time.deltaTime;
            if (droneLevel > 0 && _droneTimer <= 0)
            {
                _droneTimer = 0.2f;
                EnemyEntity target = FindTarget(transform.position, 12f);
                if (target != null)
                {
                    foreach (Transform drone in _drones)
                    {
                        Vector3 direction = target.transform.position + Vector3.up - drone.position;
                        _fire.FireItemBullet(drone.position, Quaternion.LookRotation(direction), _stats.Damage * 0.45f);
                    }
                    _droneTimer = 1.2f;
                }
            }
            int mineLevel = GetLevel(ItemType.Mine);
            _mineTimer -= Time.deltaTime;
            if (mineLevel > 0 && _mineTimer <= 0 && Vector3.Distance(_lastMinePosition, transform.position) >= 1f)
            {
                _lastMinePosition = transform.position;
                ItemMine.Spawn(transform.position + Vector3.up * 0.1f, mineLevel, _stats.Damage);
                _mineTimer = 5 - mineLevel * 0.7f;
            }
        }

        private static EnemyEntity FindTarget(Vector3 position, float range)
        {
            EnemyEntity nearest = null;
            float distance = range * range;
            foreach (Collider hit in Physics.OverlapSphere(position, range))
            {
                EnemyEntity enemy = hit.GetComponentInParent<EnemyEntity>();
                if (enemy == null || !enemy.isActiveAndEnabled || enemy.IsDead) continue;
                float sqr = (enemy.transform.position - position).sqrMagnitude;
                if (sqr >= distance) continue;
                // Avoid shooting through level geometry.
                if (Physics.Linecast(position, enemy.transform.position + Vector3.up, out RaycastHit obstacle) &&
                    obstacle.collider.GetComponentInParent<EnemyEntity>() != enemy) continue;
                distance = sqr;
                nearest = enemy;
            }
            return nearest;
        }
    }
}

