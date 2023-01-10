using System.Collections;
using BulletPro;
using Player;
using UnityEngine;

namespace Enemies
{
    public class Shooter : MonoBehaviour, IDamageable
    {
        [SerializeField] private Health health;
        [SerializeField] private BulletEmitter[] emitterList;

        [Header("Health")]
        [SerializeField] private int maxHealth = 5;
        
        [Header("Timing")]
        [SerializeField] private float timeBeforeFirstFire;
        [SerializeField] private float shootCooldown;
        [SerializeField, Range(1,4)] private int level;

        private BulletEmitter emitter;
        private Transform target;
        private Vector2 lookDirection;

        private void Awake()
        {
            emitter = emitterList[level - 1];
            target = FindObjectOfType<PlayerController>().transform;
        }
        
        private void Start()
        {
            health.Init(maxHealth);
            StartCoroutine(FirstFireDelay());
        }

        private void Update()
        {
            lookDirection = target.position - transform.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle-90);
        }
        
        private IEnumerator FirstFireDelay()
        {
            yield return new WaitForSeconds(timeBeforeFirstFire);
            StartCoroutine(FireLoop());
        }

        private IEnumerator FireLoop()
        {
            emitter.Play();
            
            float timer = shootCooldown;
            while (timer > 0)
            {
                yield return null;
                timer -= Time.deltaTime * Energy.GameSpeed;
            }
            
            emitter.Stop();
            StartCoroutine(FireLoop());
        }
        
        public void Damage(BulletPro.Bullet bullet, Vector3 hitPoint)
        {
            health.LoseHealth(bullet.moduleParameters.GetInt("Damage"));
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
}
