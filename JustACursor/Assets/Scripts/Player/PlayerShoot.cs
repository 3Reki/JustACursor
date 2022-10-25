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

            // todo : fix sprite rotation
            GameObject bulletGO = Pooler.instance.Pop(Pooler.Key.Bullet, firePoint.position, firePoint.rotation);
            bulletGO.GetComponent<Bullet>().Shoot(playerController.transform.right);

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