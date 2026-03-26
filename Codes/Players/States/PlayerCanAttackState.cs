using _01_Scripts.Entities;
using _01_Work.JY._01_Scripts.Manager;
using UnityEngine;

namespace _01_Scripts.Players.States
{
    public class PlayerCanAttackState : PlayerState
    {
        private readonly PlayerEnemyDetect _detect;
        private readonly PlayerFire _playerFire;
        private readonly PlayerStat _playerStat;

        private float _curTime;
        private bool _hasFired;

        private const float FireDelay = 0.2f;
        private const float ResetDelay = 0.4f;

        public PlayerCanAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _detect = entity.GetCompo<PlayerEnemyDetect>();
            _playerFire = entity.GetCompo<PlayerFire>();
            _playerStat = entity.GetCompo<PlayerStat>();
        }

        public override void Update()
        {
            if (_detect.Colliders != null)
            {
                if (_detect.Colliders.Length > 0 && !_detect.isObstacle)
                {
                    _curTime += Time.deltaTime;

                    if (_curTime >= _playerStat.AttackSpeed)
                    {
                        _movement.RotateTarget();
                        HandleAttackTiming();
                    }
                }
                else
                {
                    _hasFired = false;
                    _curTime = 0;
                }
            }
        }

        private void HandleAttackTiming()
        {
            if (!_movement.CanShoot) return;

            float fireTime = _playerStat.AttackSpeed + FireDelay;
            float resetTime = _playerStat.AttackSpeed + ResetDelay;

            if (!_hasFired && _curTime >= fireTime && _curTime < resetTime)
            {
                _playerFire.FireBullet(_detect.DamageCalc());
                _hasFired = true;
                AudioManager.Instance.PlaySfx("FIRE");
            }

            if (_curTime >= resetTime)
            {
                _hasFired = false;
                _movement.CanShoot = false;
                _curTime = 0;
            }
        }
    }
}