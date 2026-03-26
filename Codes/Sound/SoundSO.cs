using _01_Work.JY._01_Scripts.Manager;
using UnityEngine;

namespace _01_Scripts.Sound
{
    [CreateAssetMenu(fileName = "SoundSO", menuName = "SO/Audio/Data", order = 0)]
    public class SoundSO : ScriptableObject
    {
        public AudioManager.SoundType soundType;
        public AudioClip clip;
        public string soundName;
    }
}