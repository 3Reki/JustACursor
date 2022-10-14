using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerShoot : MonoBehaviour
{
    private PlayerInput inputs;

    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate;
    
    private bool canShoot = true;

    private void Start()
    {
        inputs = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (!inputs.GetActionPressed(PlayerInput.InputAction.Shoot) || !canShoot) return;

        GameObject bulletGO = Pooler.Instance.Pop("Bullet", firePoint.position,
            firePoint.rotation * Quaternion.Euler(0, 0, -90)); // todo : fix sprite rotation
        bulletGO.GetComponent<Bullet>().Shoot();

        canShoot = false;
        StartCoroutine(ShootCooldown());
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(1f / fireRate);
        canShoot = true;
    }
}