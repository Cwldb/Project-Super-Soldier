using _01_Scripts.Combat;
using _01_Scripts.Entities;
using DG.Tweening;
using KJYLib.StatSystem;
using UnityEngine;
using UnityEngine.AI;

namespace _01_Scripts.Enemy
{
    public class NavMovement : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private StatSO moveSpeedStat;
        [SerializeField] private float stopOffset = 0.05f;
        [SerializeField] private float rotationSpeed = 10f;

        public Entity Entity { get; set; }
        private EntityStat _statCompo;

        public bool IsArrived => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + stopOffset;

        public float RemainDistance => agent.pathPending ? -1 : agent.remainingDistance;
        
        public void Initialize(Entity entity)
        {
            this.Entity = entity;
            _statCompo = entity.GetCompo<EntityStat>();
        }


        public void AfterInitialize()
        {
            agent.speed = _statCompo.SubscribeStat(moveSpeedStat, HandleMoveSpeedChange, 1f);
        }

        private void HandleMoveSpeedChange(StatSO stat, float currentValue, float prevValue)
        {
            agent.speed = currentValue;
        }

        private void OnDestroy()
        {
            Entity.transform.DOKill();
            _statCompo.UnSubscribeStat(moveSpeedStat, HandleMoveSpeedChange);
        }

        private void Update()
        {
            if (agent.hasPath && agent.isStopped == false && agent.path.corners.Length > 0)
            {
                LookAtTarget(agent.steeringTarget, true);
            }
        }

        public void LookAtTarget(Vector3 target, bool isSmooth = true)
        {
            Vector3 direction = target - Entity.transform.position;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);

            if (isSmooth)
                Entity.transform.rotation = Quaternion.Slerp(Entity.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            else
                Entity.transform.rotation = lookRotation;
        }

        public void SetStop(bool isStop)
        {
            if(agent != null && agent.enabled && agent.isOnNavMesh)
                agent.isStopped = isStop;
        }
            
        public void SetVelocity(Vector3 velocity) => agent.velocity = velocity;
        public void SetSpeed(float speed) => agent.speed = speed;
        public void SetDestination(Vector3 destination) => agent.SetDestination(destination);

    }
}