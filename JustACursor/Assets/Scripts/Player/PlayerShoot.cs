using System.Collections;
using BulletPro;
using UnityEngine;

namespace Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private BulletEmitter emitter;

        [Header("Feedback")]
        [SerializeField] private GameObject psMuzzle;

        private bool canShoot = true;
        
        private PlayerData data => playerController.Data;
        
        public void Shoot()
        {
            if (!canShoot) return;
            
            StartCoroutine(ShootCooldown());
            psMuzzle.SetActive(true);
        }

        public void StopShoot()
        {
            psMuzzle.SetActive(false);
        }

        private IEnumerator ShootCooldown()
        {
            canShoot = false;
            emitter.Play();
            
            float shootCooldown = 1 / data.fireRate;
            while (shootCooldown > 0)
            {
                yield return null;
                shootCooldown -= Time.deltaTime * Energy.GameSpeed;
            }
            
            emitter.Stop();
            canShoot = true;
        }
    }
}