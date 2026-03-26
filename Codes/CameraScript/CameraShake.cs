using System.Collections;
using _01_Scripts.Core;
using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;

namespace _01_Scripts.CameraScript
{
    public class CameraShake : MonoSingleton<CameraShake>
    {
        private CinemachineBasicMultiChannelPerlin virtualCamera;

        private void Start()
        {
            virtualCamera = GetComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void Shake()
        {
            virtualCamera.AmplitudeGain = 5;
            virtualCamera.FrequencyGain = 5;
            StartCoroutine(EndShake());
        }

        public void Active1Shake()
        {
            virtualCamera.AmplitudeGain = 10;
            virtualCamera.FrequencyGain = 10;
            StartCoroutine(EndActiveShake());
        }
        
        public void Active2Shake()
        {
            virtualCamera.AmplitudeGain = 15;
            virtualCamera.FrequencyGain = 15;
            StartCoroutine(EndActiveShake());
        }

        private IEnumerator EndShake()
        {
            yield return new WaitForSeconds(0.1f);
            virtualCamera.AmplitudeGain = 0;
            virtualCamera.FrequencyGain = 0;
        }
        
        private IEnumerator EndActiveShake()
        {
            yield return new WaitForSeconds(0.5f);
            virtualCamera.AmplitudeGain = 0;
            virtualCamera.FrequencyGain = 0;
        }
    }
}