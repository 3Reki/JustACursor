using System.Collections;
using LD;
using MyBox;
using UnityEngine;

namespace Enemies
{
    public class Glitcher : MonoBehaviour, IDamageable
    {
        [SerializeField] private GameObject aoePrefab;
        [SerializeField] private AreaOfEffect aoeParent;
        [SerializeField] private Transform childTransform;
        [SerializeField] private Health health;
        [SerializeField] private BasicMovement movement;

        [Header("Health")]
        [SerializeField] private int maxHealth = 5;
        
        [Header("Timing")]
        [SerializeField] private float timeBeforeFirstFire;
        [SerializeField] private float previewDuration;
        [SerializeField] private float aoeDuration;
        [SerializeField] private float aoeCooldown;

        [Header("Area of Effects")]
        [SerializeField, Range(1,3)] private int level;
        
        [ConditionalField(nameof(level), true, 2)]
        [SerializeField] private float parentRadius = 2;
        [ConditionalField(nameof(level), true, 1)]
        [SerializeField] private int childNumber = 4;
        [ConditionalField(nameof(level), true, 1)]
        [SerializeField] private float childRadius = 1;
        [ConditionalField(nameof(level), true, 1)]
        [SerializeField] private float childSpawnRadius = 3;

        [Header("Editor Preview")]
        [SerializeField] private bool showPreview;

        private AreaOfEffect[] childAoE;

        private void Awake()
        {
            aoeParent.SetRadius(parentRadius);

            if (level == 1) return;
            
            childAoE = new AreaOfEffect[childNumber];
            for (int i = 0; i < childAoE.Length; i++)
            {
                childAoE[i] = Instantiate(aoePrefab,childTransform).GetComponent<AreaOfEffect>();
                childAoE[i].SetRadius(childRadius);
            }
        }

        private void Start()
        {
            health.Init(maxHealth);
            StartCoroutine(FirstFireDelay());
        }
        
        private IEnumerator FirstFireDelay()
        {
            yield return new WaitForSeconds(timeBeforeFirstFire);
            StartCoroutine(FireLoop());
        }

        private IEnumerator FireLoop()
        {
            if (level != 2) aoeParent.StartFire(previewDuration, aoeDuration);

            if (level != 1)
            {
                foreach (AreaOfEffect aoe in childAoE)
                {
                    Vector2 spawnPosition = Random.insideUnitCircle * childSpawnRadius;
                    aoe.StartFire(previewDuration, aoeDuration, spawnPosition);
                }
            }

            float timer = previewDuration+aoeDuration;
            while (timer > 0)
            {
                yield return null;
                timer -= Time.deltaTime * Energy.GameSpeed;
            }
            
            timer = aoeCooldown;
            while (timer > 0)
            {
                yield return null;
                timer -= Time.deltaTime * Energy.GameSpeed;
            }
            
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
        
        private void OnDrawGizmosSelected()
        {
            if (!showPreview) return;

            var position = transform.position;
            if (level != 1)
            {
                Gizmos.color = new Color(0,0,1,.5f);
                Gizmos.DrawSphere(position, childSpawnRadius);
            }

            if (level != 2)
            {
                Gizmos.color = new Color(1,0,0,.5f);
                Gizmos.DrawSphere(position, parentRadius);
            }

            if (level != 1)
            {
                Gizmos.color = new Color(0,1,0,.5f);
                Gizmos.DrawSphere(position, childRadius);
            }
        }
    }
}