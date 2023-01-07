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
        
        public void StartShoot()
        {
            if (!canShoot) return;
            
            StartCoroutine(ShootCR());
            psMuzzle.SetActive(true);
        }

        public void StopShoot()
        {
            psMuzzle.SetActive(false);
        }

        private IEnumerator ShootCR()
        {
            emitter.Play();
            yield return ShootCooldown();
            emitter.Stop();

            if (playerController.IsShooting)
                StartCoroutine(ShootCR());
        }

        private IEnumerator ShootCooldown()
        {
            canShoot = false;
            
            float shootCooldown = 1 / data.fireRate;
            while (shootCooldown > 0)
            {
                yield return null;
                shootCooldown -= Time.deltaTime * Energy.GameSpeed;
            }

            canShoot = true;
        }
    }
}