using System;
using _01_Scripts.Entities;
using KJYLib.StatSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _01_Scripts.Players
{
    public class PlayerEnemyDetect : MonoBehaviour, IEntityComponent
    {
        public StatSO atkPowerStat, critPowerStat, critPerStat;
        
        public float radius = 0f;
        [SerializeField] private LayerMask layer;
        [SerializeField] private LayerMask obstacleLayer;
        
        public Collider[] Colliders { get; set; }
        public Collider ShortEnemy { get; set; }

        public bool isObstacle { get; set; }

        private Entity _entity;
        private EntityStat _statCompo;
        private CharacterMovement _movement;
        private PlayerStat _playerStat;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = entity.GetCompo<EntityStat>();
            _movement = entity.GetCompo<CharacterMovement>();
            _playerStat = entity.GetCompo<PlayerStat>();
        }
        
        public float DamageCalc()
        {
            float dmg = _playerStat.Damage;
            if (Random.value < _playerStat.CritPer)
            {
                return dmg *= _playerStat.CritPower;
            }
            
            return dmg;
        }

        private void FixedUpdate()
        {
            if (_movement.CanShoot) return;

            ShortEnemy = null;
            isObstacle = false;

            float shortestDistance = Mathf.Infinity;
            Colliders = Physics.OverlapSphere(transform.position, radius, layer);

            foreach (var col in Colliders)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                Vector3 dir = (col.transform.position - transform.position).normalized;

                if (distance < shortestDistance 
                    && !Physics.Raycast(transform.position, dir, distance, obstacleLayer) 
                    && Physics.Raycast(transform.position, dir, distance, layer))
                {
                    shortestDistance = distance;
                    ShortEnemy = col;
                }
            }

            isObstacle = ShortEnemy == null && Colliders.Length > 0;
        }
    }
}