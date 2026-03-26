using System;
using Blade.Effects;
using KJYLib.ObjectPool.RunTime;
using UnityEngine;

namespace _01_Scripts.Effect
{
    public class PoolingEffect : MonoBehaviour, IPoolable
    {
        [field : SerializeField] public PoolItemSO PoolItem { get; private set; }

        // GameObject 프로퍼티에서 오브젝트가 파괴되었는지 체크하여 예외 방지
        public GameObject GameObject
        {
            get
            {
                if (this == null || gameObject == null)
                {
                    Debug.LogWarning("PoolingEffect: GameObject is destroyed or missing.");
                    return null;
                }
                return gameObject;
            }
        }

        private Pool _myPool;
        [SerializeField] private GameObject effectObject;
        private IPlayableVFX _playableVFX;

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
            _playableVFX = effectObject.GetComponent<IPlayableVFX>();
            Debug.Assert(_playableVFX != null, "effect object must have IPlayableVFX component");
        }

        public void ResetItem()
        {
            _playableVFX.StopVFX();
        }

        public void PlayerVFX(Vector3 position, Quaternion rotation)
        {
            _playableVFX.PlayVFX(position, rotation);
        }

        private void OnValidate()
        {
            if (effectObject == null) return;
            _playableVFX = effectObject.GetComponent<IPlayableVFX>();
            if(_playableVFX == null)
            {
                effectObject = null;
                Debug.LogError("effect object must have IPlayableVFX component");
            }
        }
    }
}
