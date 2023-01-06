using System;
using System.Collections;
using System.Collections.Generic;
using BulletBehaviour;
using CameraScripts;
using LD;
using UnityEngine;

namespace Player {
    public class PlayerCollision : MonoBehaviour, IDamageable
    {
        [HideInInspector] public bool IsInvincible;
        
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PolygonCollider2D playerCollider;

        private List<BulletPro.Bullet> shockwaveCollisions = new();
        private List<BoxCollider2D> laserCollisions = new();
        
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

        private void Update()
        {
            CheckLaserCollisions();
        }
        
        private void Shake()
        {
            CameraController.ShakeCamera(data.onHitShakeIntensity, data.onHitShakeDuration);
        }
        
        public void Damage(int damage = 1)
        {
            if (IsInvincible) return;
            
            health.LoseHealth(damage);
            StartCoroutine(Invincibility());
        }

        private IEnumerator Invincibility()
        {
            IsInvincible = true;
            yield return new WaitForSeconds(data.invinciblityTime);
            IsInvincible = false;
        }

        private void CheckLaserCollisions()
        {
            foreach (BoxCollider2D laserCollider in laserCollisions)
            {
                if (!playerCollider.bounds.Intersects(laserCollider.bounds)) continue;
                
                Damage();
                break;
            }
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

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out Laser laser))
            {
                laserCollisions.Add(laser.GetComponent<BoxCollider2D>());
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Laser laser))
            {
                laserCollisions.Remove(laser.GetComponent<BoxCollider2D>());
            }
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
    }
}

