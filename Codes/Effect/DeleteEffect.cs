using System;
using System.Collections;
using UnityEngine;

namespace _01_Scripts.Effect
{
    public class DeleteEffect : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(DeleteEffectTime());
        }

        private IEnumerator DeleteEffectTime()
        {
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }
    }
}