using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Transform firePoint;
    
        private bool canShoot = true;

        public void Shoot()
        {
            if (!canShoot) return;

            GameObject bulletGO = Pooler.instance.Pop("Bullet", firePoint.position,
                firePoint.rotation * Quaternion.Euler(0, 0, -90)); // todo : fix sprite rotation
            bulletGO.GetComponent<Bullet>().Shoot();

            canShoot = false;
            StartCoroutine(ShootCooldown());
        }

        private IEnumerator ShootCooldown()
        {
            yield return new WaitForSeconds(1f / playerController.data.fireRate);
            canShoot = true;
        }
    }
}