using _01_Scripts.Entities;
using UnityEngine;

namespace _01_Scripts.Players.States
{
    public class PlayerMoveState : PlayerCanAttackState
    {
        public PlayerMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }
        public override void Update()
        {
            base.Update();
            Vector2 movementKey = _player.PlayerInput.MovementKey;
            
            _movement.SetMovementDirection(movementKey);
            if(movementKey.magnitude < _inputThreshold)
                _player.ChangeState("IDLE");
        }
    }
}