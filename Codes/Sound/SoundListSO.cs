using System.Collections.Generic;
using UnityEngine;

namespace _01_Scripts.Sound
{
    [CreateAssetMenu(fileName = "SoundList", menuName = "SO/Audio/list", order = 0)]
    public class SoundListSO : ScriptableObject
    {
        public List<SoundSO> soundDataList = new List<SoundSO>();
    }
}