using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _01_Scripts.Players
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "SO/PlayerInput", order = 0)]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        [SerializeField] private LayerMask whatIsGround;
        // public event Action<Vector2> OnMovementChange;
        public event Action OnAttackPressed;
        public event Action OnActive1Pressed;
        public event Action OnActive1Released;
        public event Action OnActive2Pressed;
        public event Action OnActive2Released;
        
        public Vector2 MovementKey { get; private set; }
        
        private Controls _controls;
        private Vector2 _screenPosition; //마우스 좌표
        private Vector3 _worldPosition;
        
        public void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }

        public void OnDisable()
        {
            _controls.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MovementKey = context.ReadValue<Vector2>();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnAttackPressed?.Invoke();
        }
        
        public void OnPointer(InputAction.CallbackContext context)
        {
            _screenPosition = context.ReadValue<Vector2>();
        }

        public void OnActiveSkill1(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnActive1Pressed?.Invoke();
            if(context.canceled)
                OnActive1Released?.Invoke();
        }

        public void OnActiveSkill2(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnActive2Pressed?.Invoke();
            if(context.canceled)
                OnActive2Released?.Invoke();
        }

        public Vector3 GetWorldPosition()
        {
            Camera mainCam = Camera.main;
            Debug.Assert(mainCam != null, "No main camera in this scene");
            Ray camRay = mainCam.ScreenPointToRay(_screenPosition);
            if (Physics.Raycast(camRay, out RaycastHit hit, mainCam.farClipPlane, whatIsGround))
            {
                _worldPosition = hit.point;
            }

            return _worldPosition;
        }
    }
}