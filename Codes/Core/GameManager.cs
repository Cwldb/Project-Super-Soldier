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
        public Action OnGameOver;
        public Action OnValueChange;

        [field: SerializeField] public EntityFinderSO PlayerFinder { get; set; }

        public int startLevelCount;
        public float enemySpawnDelay;
        
        private int _enemyCount = 0;
        private int _curLevel = 1;

        public int GetCurNeedLevel() => startLevelCount - _enemyCount;
        public int GetCurLevel() => _curLevel;

        public void AddEnemyCount(int cnt = 1)
        {
            _enemyCount += cnt;
            if (_enemyCount == startLevelCount)
            {
                if (_curLevel % 3 == 0)
                {
                    OnActiveLevelUp?.Invoke();
                }
                else
                    OnLevelUp?.Invoke();
                _enemyCount = 0;
                _curLevel++;
                startLevelCount += 10;
            }
            OnEnemyCount?.Invoke();
        }
    }
}
