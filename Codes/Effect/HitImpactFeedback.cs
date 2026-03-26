using _01_Scripts.Entities;
using Assets.Blade.Feedbacks;
using KJYLib.Dependencies;
using KJYLib.ObjectPool.RunTime;
using UnityEngine;

namespace _01_Scripts.Effect
{
    public class HitImpactFeedback : FeedBack
    {
        [SerializeField] private PoolItemSO hitImpact;
        [SerializeField] private float playDuration = 0.2f;

        [Inject] private PoolManagerMono _poolManager;

        private PoolingEffect _effect;

        public override async void CreateFeedback()
        {
            PoolingEffect effect = _poolManager.Pop<PoolingEffect>(hitImpact);

            await Awaitable.WaitForSecondsAsync(playDuration);
            _poolManager.Push(effect);

        }

        public override void StopFeedback()
        {

        }
    }
}