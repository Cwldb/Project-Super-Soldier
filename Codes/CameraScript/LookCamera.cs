using System;
using UnityEngine;

namespace _01_Scripts.CameraScript
{
    public class LookCamera : MonoBehaviour
    {
        public Transform enemyTransform; // 적 위치
        public float heightOffset = 2f; // 머리 위
        public float forwardOffset = 1f; // 카메라 기준 앞 거리

        void LateUpdate()
        {
            if (enemyTransform == null) return;

            // 카메라가 적을 바라보는 방향
            Vector3 camToEnemyDir = (enemyTransform.position - Camera.main.transform.position).normalized;

            // 위치 계산 (카메라 기준 앞 + 위)
            Vector3 targetPos = enemyTransform.position + camToEnemyDir * forwardOffset + Vector3.up * heightOffset;

            transform.position = targetPos;

            // 항상 카메라 바라보게
            transform.forward = Camera.main.transform.forward;
        }
    }
}