using UnityEngine;

namespace _01_Scripts.Entities
{
    public class EntityActionData : MonoBehaviour, IEntityComponent
    {
        public Vector3 HitPoint { get; set; }
        public Vector3 HitNormal { get; set; }

        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
    }
}