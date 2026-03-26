using UnityEngine;

namespace _01_Scripts.FSM
{
    [CreateAssetMenu(fileName = "StateData", menuName = "SO/StateData", order = 0)]
    public class StateDataSO : ScriptableObject
    {
        public string stateName;
        public string className;
        public string animParamName;
        //hash는 public 으로 안하면 저장이 안되서 런타임에서 에러가 생긴다. 주의!
        public int animationHash;

        private void OnValidate()
        {
            animationHash = Animator.StringToHash(animParamName);
        }
    }
}