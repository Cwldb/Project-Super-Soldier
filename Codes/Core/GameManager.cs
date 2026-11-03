using System;
using _01_Scripts.Entities;
using UnityEngine;

namespace _01_Scripts.Core
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public Action OnLevelUp;
        public Action OnActiveLevelUp;
        public Action OnEnemyCount;
        public event Action OnExperienceChanged;
        public Action OnGameOver;
        public Action OnValueChange;

        [field: SerializeField] public EntityFinderSO PlayerFinder { get; set; }

        // Keep the serialized field so existing scene progression settings are preserved.
        [Min(1)] public int startLevelCount = 5;
        public float enemySpawnDelay;
        
        private int _experience;
        private bool _levelUpPending;
        private int _curLevel = 1;

        public int CurrentExperience => _experience;
        public int RequiredExperience => Mathf.Max(1, startLevelCount);
        public float ExperienceProgress => Mathf.Clamp01((float)_experience / RequiredExperience);
        public int GetCurNeedLevel() => Mathf.Max(0, RequiredExperience - _experience);
        public int GetCurLevel() => _curLevel;

        public void AddExperience(int amount)
        {
            if (amount <= 0) return;
            _experience += amount;
            TryLevelUp();
            OnExperienceChanged?.Invoke();
            OnEnemyCount?.Invoke();
        }

        // Compatibility for any existing UnityEvent bindings.
        public void AddEnemyCount(int cnt = 1) => AddExperience(cnt);

        private void TryLevelUp()
        {
            if (_levelUpPending || _experience < RequiredExperience) return;
            _experience -= RequiredExperience;
            bool activeUpgrade = _curLevel % 3 == 0;
            _curLevel++;
            startLevelCount = RequiredExperience + 10;
            _levelUpPending = true;
            if (activeUpgrade) OnActiveLevelUp?.Invoke();
            else OnLevelUp?.Invoke();
        }

        public void CompleteLevelUp()
        {
            if (!_levelUpPending) return;
            _levelUpPending = false;
            TryLevelUp();
            OnExperienceChanged?.Invoke();
        }
    }
}
