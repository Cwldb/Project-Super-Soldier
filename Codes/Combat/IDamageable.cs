using _01_Scripts.Entities;
using UnityEngine;

namespace _01_Scripts.Combat
{
    public interface IDamageable
    {
        public void ApplyDamage(Vector3 hitPoint, Vector3 hitNormal, AttackDataSO attackData, Entity dealer);
    }
}