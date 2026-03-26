using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _01_Scripts.Core;
using _01_Scripts.Enemy;
using KJYLib.Dependencies;
using KJYLib.ObjectPool.RunTime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _01_Scripts.Spawner
{
    public class EnemySpawner : MonoBehaviour
    {
        [System.Serializable]
        public class SpawnableEnemy
        {
            public PoolItemSO enemy;
            [Range(0f, 1f)] public float probability;
        }

        [SerializeField] private List<SpawnableEnemy> spawnableEnemies = new List<SpawnableEnemy>();
        private List<Transform> enemySpawnPos = new List<Transform>();

        [SerializeField] private float xMin = -10f;
        [SerializeField] private float xMax = 10f;
        [SerializeField] private float zMin = -10f;
        [SerializeField] private float zMax = 10f;
        
        [Inject] private PoolManagerMono _poolManagerMono;
        
        
        private void Start()
        {
            foreach (Transform trm in transform)
                enemySpawnPos.Add(trm);

            TimerManager.Instance.OnMiddleBossSpawn += HandleSpawnMiddleBoss;
            TimerManager.Instance.OnBossSpawn += HandleSpawnBoss;

            StartCoroutine(SpawnEnemy());
        }

        private void HandleSpawnMiddleBoss()
        {
            Transform spawnPoint = GetValidSpawnPoint();
            if (spawnPoint != null)
            {
                EnemySoldiers enemy = _poolManagerMono.Pop<EnemySoldiers>(spawnableEnemies[3].enemy);
                enemy.transform.position = spawnPoint.position;
            }
        }

        private void HandleSpawnBoss()
        {
            Debug.Log("Boss Spawned!");   
            Transform spawnPoint = GetValidSpawnPoint();
            if (spawnPoint != null)
            {
                EnemySoldiers enemy = _poolManagerMono.Pop<EnemySoldiers>(spawnableEnemies[4].enemy);
                enemy.transform.position = spawnPoint.position;
            }
            StartCoroutine(SpawnEnezmyBoss());
        }

        private IEnumerator SpawnEnezmyBoss()
        {
            yield return new WaitForSeconds(120);
            TimerManager.Instance.isBossSpawn = false;
        }

        private IEnumerator SpawnEnemy()
        {
            Transform spawnPoint = GetValidSpawnPoint();
            if (spawnPoint != null)
            {
                PoolItemSO selectedEnemy = GetEnemyByProbability();
                if (selectedEnemy != null)
                {
                    EnemySoldiers enemy = _poolManagerMono.Pop<EnemySoldiers>(selectedEnemy);
                    enemy.transform.position = spawnPoint.position;
                }
            }

            yield return new WaitForSeconds(GameManager.Instance.enemySpawnDelay);
            StartCoroutine(SpawnEnemy());
        }

        private Transform GetValidSpawnPoint()
        {
            List<Transform> validSpawnPoints = enemySpawnPos.Where(pos =>
                pos.position.x >= xMin &&
                pos.position.x <= xMax &&
                pos.position.z >= zMin &&
                pos.position.z <= zMax
            ).ToList();

            if (validSpawnPoints.Count == 0)
                return null;

            return validSpawnPoints[Random.Range(0, validSpawnPoints.Count)];
        }

        private PoolItemSO GetEnemyByProbability()
        {
            float total = spawnableEnemies.Sum(e => e.probability);
            float random = Random.Range(0f, total);

            float cumulative = 0f;
            foreach (var data in spawnableEnemies)
            {
                cumulative += data.probability;
                if (random <= cumulative)
                    return data.enemy;
            }

            return null;
        }
    }
}