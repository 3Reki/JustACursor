using System;
using System.Collections;
using CameraScripts;
using UnityEngine;

namespace Player {
    public class PlayerCollision : MonoBehaviour, IDamageable
    {
        public bool isInvincible;
        
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Health health;

        private PlayerData data => playerController.data;

        private void OnEnable()
        {
            health.onHealthLose.AddListener(Shake);
        }

        private void OnDisable()
        {
            health.onHealthLose.RemoveListener(Shake);
        }
        
        private void Awake()
        {
            health.Init(data.maxHealth);
        }

        private void Shake()
        {
            CameraController.ShakeCamera(data.onHitShakeIntensity, data.onHitShakeDuration);
        }

        public void Damage(BulletPro.Bullet bullet, Vector3 hitPoint)
        {
            if (isInvincible) return;
            
            health.LoseHealth(bullet.moduleParameters.GetInt("Damage"));
            StartCoroutine(Invincibility());
        }

        private IEnumerator Invincibility()
        {
            isInvincible = true;
            yield return new WaitForSeconds(data.invinciblityTime);
            isInvincible = false;
        }
    }
}

