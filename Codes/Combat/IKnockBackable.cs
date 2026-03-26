using UnityEngine;

namespace _01_Scripts.Combat
{
    public interface IKnockBackable
    {
        public void KnockBack(Vector3 force, float time);
    }
}
