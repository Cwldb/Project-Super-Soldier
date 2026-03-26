using System.Collections.Generic;
using System.Linq;
using Blade.Effects;
using UnityEngine;

namespace _01_Scripts.Entities
{
    public class EntityVFX : MonoBehaviour, IEntityComponent
    {
        private Dictionary<string, IPlayableVFX> _playableDictionary;
        private Entity _entity;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _playableDictionary = new Dictionary<string, IPlayableVFX>();
            GetComponentsInChildren<IPlayableVFX>().ToList()
                .ForEach(playable => _playableDictionary.Add(playable.VFXName, playable));
        }

        public void PlayVFX(string vfxName, Vector3 position, Quaternion rotation)
        {
            IPlayableVFX vfx = _playableDictionary.GetValueOrDefault(vfxName);
            Debug.Assert(vfx != default(IPlayableVFX), $"{vfxName} is not exist");
            
            vfx.PlayVFX(position, rotation);
        }

        public void StopVFX(string vfxName)
        {
            IPlayableVFX vfx = _playableDictionary.GetValueOrDefault(vfxName);
            Debug.Assert(vfx != default(IPlayableVFX), $"{vfxName} is not exist");
            
            vfx.StopVFX();
        }
    }
}