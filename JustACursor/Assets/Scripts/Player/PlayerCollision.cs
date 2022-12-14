using System.Collections;
using BulletBehaviour;
using CameraScripts;
using UnityEngine;

namespace Player {
    public class PlayerCollision : MonoBehaviour, IDamageable
    {
        [HideInInspector] public bool IsInvincible;
        
        [SerializeField] private PlayerController playerController;
        
        private PlayerData data => playerController.Data;
        private Health health => playerController.Health;

        private void OnEnable()
        {
            health.onHealthLose.AddListener(Shake);
        }

        private void OnDisable()
        {
            health.onHealthLose.RemoveListener(Shake);
        }
        
        private void Start()
        {
            health.Init(data.maxHealth);
        }

        public void Damage(BulletPro.Bullet bullet, Vector3 hitPoint)
        {
            if (IsInvincible) return;
            
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
            if (IsInvincible) return;
            
            health.LoseHealth(damage);
            StartCoroutine(Invincibility());
        }

        private void Shake()
        {
            CameraController.ShakeCamera(data.onHitShakeIntensity, data.onHitShakeDuration);
        }

        private IEnumerator Invincibility()
        {
            IsInvincible = true;
            yield return new WaitForSeconds(data.invinciblityTime);
            IsInvincible = false;
        }
    }
}

