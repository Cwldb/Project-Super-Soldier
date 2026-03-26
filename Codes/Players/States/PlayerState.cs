using _01_Scripts.Entities;
using _01_Scripts.FSM;

namespace _01_Scripts.Players.States
{
    public abstract class PlayerState : EntityState
    {
        protected Player _player;
        protected readonly float _inputThreshold = 0.1f;
        protected CharacterMovement _movement;
        
        protected PlayerState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _player = entity as Player;
            _movement = entity.GetCompo<CharacterMovement>();
        }
    }
}