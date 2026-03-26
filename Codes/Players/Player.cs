using _01_Scripts.Core;
using _01_Scripts.Entities;
using _01_Scripts.FSM;
using KJYLib.Dependencies;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _01_Scripts.Players
{
    
    public class Player : Entity, IDependencyProvider
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        [SerializeField] private StateDataSO[] states;
        [SerializeField] private GameObject skillRadius;
        
        private EntityStateMachine _stateMachine;
        private PlayerSkillSet _skillSet;

        private bool _isActive1P;
        private bool _isActive2P;

        [Provide]
        public Player ProvidePlayer() => this;
        
        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new EntityStateMachine(this, states);
            _skillSet = GetComponentInChildren<PlayerSkillSet>();

            PlayerInput.OnActive1Pressed += HandleActive1Pressed;
            PlayerInput.OnActive2Pressed += HandleActive2Pressed;
            
            PlayerInput.OnActive1Released += HandleActive1Released;
            PlayerInput.OnActive2Released += HandleActive2Released;
        }

        private void OnDisable()
        {
            PlayerInput.OnActive1Pressed -= HandleActive1Pressed;
            PlayerInput.OnActive2Pressed -= HandleActive2Pressed;

            PlayerInput.OnActive1Released -= HandleActive1Released;
            PlayerInput.OnActive2Released -= HandleActive2Released;
        }

        private void OnEnable()
        {
            PlayerInput.OnEnable();
        }

        private void HandleActive1Pressed()
        {
            if (!_skillSet.CanUseActive1) return;
            if (_isActive2P) return;
            skillRadius.SetActive(true);
            _isActive1P = true;
            skillRadius.transform.localScale = new Vector3(_skillSet.radius * 2, 0.1f, _skillSet.radius * 2);
        }
        
        private void HandleActive2Pressed()
        {
            if (!_skillSet.CanUseActive2) return;
            if (_isActive1P) return;
            _isActive2P = true;
            skillRadius.SetActive(true);
            skillRadius.transform.localScale = new Vector3(_skillSet.bombSpawnRange * 2, 0.1f, _skillSet.bombSpawnRange * 2);
        }

        private void HandleActive1Released()
        {
            if (!_isActive1P) return;
            if (_skillSet.CanUseActive1)
                ChangeState("ACTIVE1");
            skillRadius.SetActive(false);
            _isActive1P = false;
        }
        
        private void HandleActive2Released()
        {
            if (!_isActive2P) return;
            if (_skillSet.CanUseActive2)
                ChangeState("ACTIVE2");
            skillRadius.SetActive(false);
            _isActive2P = false;
        }

        protected override void Start()
        {
            base.Start();
            _stateMachine.ChangeState("IDLE");
            OnDeadEvent.AddListener(HandleDeadEvent);
        }

        private void HandleDeadEvent()
        {
            //_stateMachine.ChangeState("DEAD");
            PlayerInput.OnDisable();
            GameManager.Instance.OnGameOver.Invoke();
        }

        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }
        
        public void ChangeState(string newStateName) => _stateMachine.ChangeState(newStateName);
        
    }
}