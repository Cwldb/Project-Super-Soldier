using System;
using _01_Scripts.Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _01_Scripts.UI
{
    public class PlayerHitBlood : MonoSingleton<PlayerHitBlood>
    {
        [SerializeField] private Sprite[] bloods;
        [SerializeField] private Image image;
        [SerializeField] private Transform parent;

        
        public void BloodEffect()
        {
            Image effect = Instantiate(image, parent);
            effect.sprite = bloods[Random.Range(0, bloods.Length)];
            effect.DOFade(0, 0.5f).OnComplete(() =>
            {
                Destroy(effect.gameObject);
            });
        }
    }
}