using System.Collections;
using TMPro;
using UnityEngine;

namespace _01_Scripts.Enemy
{
    public class EnemyHitText : MonoBehaviour
    {  
        [SerializeField] private TMP_Text _text;

        public void DamageText(float damage)
        {
            _text.text = $"{damage}";
            StartCoroutine(DestroyText());
        }

        private IEnumerator DestroyText()
        {
            yield return new WaitForSeconds(0.7f);
            _text.text = "";
        }
    }
}