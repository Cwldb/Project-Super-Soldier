using System;
using UnityEngine;

namespace _01_Scripts.Entities
{
    public class EntityAnimatorTrigger : MonoBehaviour, IEntityComponent
    {
        public Action OnAnimationEndTrigger;
        public Action<bool> OnRollingStatusChange;
        public Action OnAttackVFXTrigger;
        public Action<bool> OnManualRotationTrigger;
        public Action OnDamageCastTrigger;
        
        public Action OnSwingDamageCastTrigger;
        public Action OnJumpingDamageCastTrigger;
        public Action OnBossJumpAttackTrigger;
        public Action OnBossSwingAttackTrigger;

        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        private void AnimationEnd() //매서드 명 오타나면 안된다. (이벤트 이름과 동일하게 만들어야 해.)
        {
            OnAnimationEndTrigger?.Invoke();
        }
        private void PlayAttackVFX() => OnAttackVFXTrigger?.Invoke();
        private void StartManualRotation() => OnManualRotationTrigger?.Invoke(true);
        private void StopManualRotation() => OnManualRotationTrigger?.Invoke(false);
        private void DamageCast() => OnDamageCastTrigger?.Invoke();
        
        private void SwingDamageCast() => OnSwingDamageCastTrigger?.Invoke();
        private void JumpingDamageCast() => OnJumpingDamageCastTrigger?.Invoke();
        private void JumpBossAttack() => OnBossJumpAttackTrigger?.Invoke();
        private void SwingBossAttack() => OnBossJumpAttackTrigger?.Invoke();
    }
}