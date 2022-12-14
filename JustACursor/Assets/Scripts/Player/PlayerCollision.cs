using System;
using System.Collections;
using BulletBehaviour;
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
        
        public void Damage(BulletPro.Bullet bullet, Vector3 hitPoint)
        {
            if (isInvincible) return;
            
            ShockwaveCollision shockwaveCollision = bullet.GetComponentInChildren<ShockwaveCollision>();
            if (shockwaveCollision != null)
            {
                if (shockwaveCollision.CheckCollision(hitPoint))
                    Damage(bullet.moduleParameters.GetInt("Damage"));
            }
            else Damage(bullet.moduleParameters.GetInt("Damage"));
        }

        public void Damage(int damage = 1)
        {
            if (isInvincible) return;
            
            health.LoseHealth(damage);
            StartCoroutine(Invincibility());
        }

        private void Shake()
        {
            CameraController.ShakeCamera(data.onHitShakeIntensity, data.onHitShakeDuration);
        }

        private IEnumerator Invincibility()
        {
            isInvincible = true;
            yield return new WaitForSeconds(data.invinciblityTime);
            isInvincible = false;
        }
    }
}

