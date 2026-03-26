using System;
using UnityEngine;

namespace _01_Scripts.Core
{
    public class TimerManager : MonoBehaviour
    {
        public static TimerManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public Action OnMiddleBossSpawn;
        public Action OnBossSpawn;
        
        public float Count { get; set; }
        public int Minutes { get; set; }
        
        public bool isDeath { get; set; }

        public bool is1Minute{ get; set; }
        public bool is2Minute{ get; set; }
        public bool is3Minute{ get; set; }
        public bool is4Minute{ get; set; }
        public bool is5Minute { get; set; }
        
        public bool isBossSpawn {get; set;}

        private void Update()
        {
            if(!isDeath)
                Count +=  Time.deltaTime;
            
            if (Count >= 60)
            {
                Minutes++;
                OnMiddleBossSpawn?.Invoke();
                Count = 0;
            }

            if (Minutes == 1 && !is1Minute)
            {
                GameManager.Instance.enemySpawnDelay -= 0.2f;
                is1Minute = true;
            }
            else if (Minutes == 2 && !is2Minute)
            {
                GameManager.Instance.enemySpawnDelay -= 0.2f;
                is2Minute = true;
            }
            else if (Minutes == 3  && !is3Minute)
            {
                GameManager.Instance.enemySpawnDelay -= 0.1f;
                is3Minute = true;
            }
            else if (Minutes == 4 && !is4Minute)
            {
                GameManager.Instance.enemySpawnDelay -= 0.1f;
                is4Minute = true;
            }
            else if (Minutes == 5 && !is5Minute)
            {
                GameManager.Instance.enemySpawnDelay -= 0.2f;
                is5Minute = true;
            }
            else if (Minutes % 2 == 0 && Minutes > 5 && !isBossSpawn)
            {
                OnBossSpawn?.Invoke();
                isBossSpawn = true;
            }
        }
    }
}