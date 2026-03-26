using UnityEngine;

namespace _01_Scripts.Entities
{
    [CreateAssetMenu(fileName = "EntityFinder", menuName = "SO/EntityFinder", order = 0)]
    public class EntityFinderSO : ScriptableObject
    {
        public Entity Target { get; private set; }

        public void SetTarget(Entity target)
        {
            Target = target;
        }
    }
}