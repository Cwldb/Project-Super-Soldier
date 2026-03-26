using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace _01_Scripts.UI.TextEffect
{
    public class TitleTextEffect : MonoBehaviour
    {
        private TMP_Text titleText;

        private void Awake()
        {
            titleText = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            titleText.DOFade(0.8f, 2f).SetLoops(-1, LoopType.Yoyo);
        }

        private void Update()
        {
            
        }
    }
}