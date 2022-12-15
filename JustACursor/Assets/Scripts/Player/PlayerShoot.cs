using System.Collections;
using BulletPro;
using UnityEngine;

namespace Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private BulletEmitter emitter;

        private PlayerData data => playerController.Data;

        private bool canShoot = true;
        
        public void Shoot()
        {
            if (!canShoot) return;
            
            emitter.Play();
            StartCoroutine(ShootCooldown());
        }

        private IEnumerator ShootCooldown()
        {
            canShoot = false;
            yield return new WaitForSeconds(1 / data.fireRate / Energy.GameSpeed);
            emitter.Stop();
            canShoot = true;
        }
    }
}