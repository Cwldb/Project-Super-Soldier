using _01_Scripts.Entities;
using UnityEngine;

namespace _01_Scripts.Players.States
{
    public class PlayerActive1State : PlayerState
    {
        private PlayerSkillSet _playerSkill;
        private float _curTime;
        public PlayerActive1State(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _playerSkill = entity.GetCompo<PlayerSkillSet>();
        }

        public override void Enter()
        {
            base.Enter();
            _curTime = 0;
            _movement.CanManualMovement = false;
            _playerSkill.UseActive1();
        }

        public override void Update()
        {
            base.Update();
            _curTime += Time.deltaTime;
             
            if (_curTime >= 0.3f)
            {
                _movement.CanManualMovement = true;
                _player.ChangeState("IDLE");
            }
        }
    }
}