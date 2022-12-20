using System.Collections;
using System.Collections.Generic;
using BulletBehaviour;
using CameraScripts;
using UnityEngine;

namespace Player {
    public class PlayerCollision : MonoBehaviour, IDamageable
    {
        [HideInInspector] public bool IsInvincible;
        
        [SerializeField] private PlayerController playerController;

        private List<BulletPro.Bullet> shockwaveCollisions = new();
        
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

        public void OnHitByBulletEnter(BulletPro.Bullet bullet, Vector3 hitPoint)
        {
            if (bullet.GetComponentInChildren<ShockwaveCollision>())
                shockwaveCollisions.Add(bullet);
        }
        
        public void Damage(BulletPro.Bullet bullet, Vector3 hitPoint)
        {
            if (IsInvincible) return;

            if (shockwaveCollisions.Contains(bullet))
            {
                ShockwaveCollision shockwaveCollision = bullet.GetComponentInChildren<ShockwaveCollision>();
                if (shockwaveCollision.CheckCollision(hitPoint))
                    Damage(bullet.moduleParameters.GetInt("Damage"));
            }
            else
            {
                Damage(bullet.moduleParameters.GetInt("Damage"));
            }
        }
        
        public void OnHitByBulletExit(BulletPro.Bullet bullet, Vector3 hitPoint)
        {
            if (bullet.GetComponentInChildren<ShockwaveCollision>())
                shockwaveCollisions.Remove(bullet);
        }

        /*public void Damage(BulletPro.Bullet bullet, Vector3 hitPoint)
        {
            if (IsInvincible) return;
            
            ShockwaveCollision shockwaveCollision = bullet.GetComponentInChildren<ShockwaveCollision>();
            if (shockwaveCollision != null)
            {
                if (shockwaveCollision.CheckCollision(hitPoint))
                    Damage(bullet.moduleParameters.GetInt("Damage"));
            }
            else Damage(bullet.moduleParameters.GetInt("Damage"));
        }*/

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

