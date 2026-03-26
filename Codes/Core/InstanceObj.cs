using UnityEngine;

namespace _01_Scripts.Core
{
    public class InstanceObj : MonoBehaviour
    {
        public static InstanceObj instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
