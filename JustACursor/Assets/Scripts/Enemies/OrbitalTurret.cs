using System.Collections;
using BulletPro;
using Player;
using UnityEngine;

namespace Enemies
{
    public class OrbitalTurret : MonoBehaviour, IDamageable
    {
        [SerializeField] private Orbital orbital;
        [SerializeField] private Health health;
        [SerializeField] private BulletEmitter emitter;
        
        private Transform target;
        private Vector2 lookDirection;

        private Coroutine fireCoroutine;
        
        private void Awake()
        {
            target = FindObjectOfType<PlayerController>().transform;
        }

        private void Update()
        {
            lookDirection = target.position - transform.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle-90);
        }

        public void Init(int maxHealth)
        {
            health.Init(maxHealth);
        }

        public void Fire()
        {
            if (fireCoroutine != null) StopCoroutine(fireCoroutine);
            fireCoroutine = StartCoroutine(FireCR());
        }

        private IEnumerator FireCR()
        {
            emitter.Play();
            yield return null;
            emitter.Stop();
        }
        
        public void Damage(BulletPro.Bullet bullet, Vector3 hitPoint)
        {
            health.LoseHealth(bullet.moduleParameters.GetInt("Damage"));
        }

        public void Die()
        {
            orbital.RemoveFromTurretList(this);
            Destroy(gameObject);
        }
    }
}
