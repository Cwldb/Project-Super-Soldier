using System.Collections.Generic;
using _01_Scripts.Core;
using _01_Scripts.Sound;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _01_Work.JY._01_Scripts.Manager
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        public enum SoundType
        {
            SFX,
            BGM
        }

        [Header("------------ Audio Source ------------")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("------------- Audio clip -------------")]
        public SoundListSO soundList;
    
        private Dictionary<string, SoundSO> _soundSfxDic = new Dictionary<string, SoundSO>();
        private Dictionary<string, SoundSO> _soundBgmDic = new Dictionary<string, SoundSO>();

        private string _curKey;

        private void Start()
        {
            foreach (SoundSO sound in soundList.soundDataList)
            {
                if(sound.soundType ==  SoundType.SFX)
                    _soundSfxDic.Add(sound.soundName, sound);
                else if (sound.soundType == SoundType.BGM)
                    _soundBgmDic.Add(sound.soundName, sound);
            }
        
            PlayBGM("BGM");
        }

        public void PlaySfx(string key)
        {
            sfxSource.PlayOneShot(_soundSfxDic[key].clip);
        }

        public void PlayBGM(string key)
        {
            if (_curKey == key) return;
            _curKey = key;
            musicSource.Stop();
            musicSource.clip = _soundBgmDic[key].clip;
            musicSource.Play();
        }
    }
}
