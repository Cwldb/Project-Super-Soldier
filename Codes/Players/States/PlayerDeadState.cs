using _01_Scripts.Entities;

namespace _01_Scripts.Players.States
{
    public class PlayerDeadState : PlayerState
    {
        public PlayerDeadState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _movement.CanManualMovement = false;
            _player.ChangeState("DEAD");
        }
    }
}
