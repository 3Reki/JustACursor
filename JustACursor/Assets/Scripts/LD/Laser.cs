using System.Collections;
using BulletPro;
using UnityEngine;

namespace LD
{
    public class Laser : MonoBehaviour
    {
        [SerializeField] private Transform myTransform;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private BulletEmitter emitter;
        
        [Header("Render")]
        [SerializeField] private Gradient previewGradient;
        [SerializeField] private Gradient laserGradient;
        
        private readonly BulletCollider[] colliders = new BulletCollider[3];
        private IEnumerator fireEnumerator;

        private void Awake()
        {
            lineRenderer.positionCount = 2;
            for (var i = 0; i < colliders.Length; i++)
            {
                colliders[i] = new BulletCollider
                {
                    colliderType = BulletColliderType.Line
                };
            }
        }

        public void StartFire(float previewDuration, float laserDuration, float customWidth, float customLength, bool hasCollision = true)
        {
            SetupLineRenderer(customWidth, customLength);
            SetupColliders(customWidth, customLength);

            fireEnumerator = Fire(previewDuration, laserDuration, hasCollision);
            StartCoroutine(fireEnumerator);
        }

        public void StopFire()
        {
            if (fireEnumerator != null)
            {
                StopCoroutine(fireEnumerator);
            }

            if (emitter.isPlaying)
            {
                emitter.Stop();
            }
            emitter.Kill();
            
            lineRenderer.gameObject.SetActive(false);
        }
        
        private IEnumerator Fire(float previewDuration, float laserDuration, bool hasCollision)
        {
            lineRenderer.gameObject.SetActive(true);
            lineRenderer.colorGradient = previewGradient;
            
            //Remove Time.deltaTime bc it's the time for the laser bullet to initialize
            while (previewDuration > 0)
            {
                yield return null;
                previewDuration -= Time.deltaTime * Energy.GameSpeed;
            }
            
            emitter.Play();
            //Wait for bullet to initialize
            yield return null;
            if (emitter.bullets.Count > 0)
            {
                SetupBullet(emitter.bullets[^1], hasCollision);
            }

            lineRenderer.colorGradient = laserGradient;
            
            laserDuration -= Time.deltaTime * Energy.GameSpeed;
            while (laserDuration > 0)
            {
                yield return null;
                laserDuration -= Time.deltaTime * Energy.GameSpeed;
            }
            
            emitter.Stop();
            emitter.Kill();
            lineRenderer.gameObject.SetActive(false);
        }

        private void SetupBullet(BulletPro.Bullet bullet, bool hasCollision)
        {
            //Collision Module
            if (!hasCollision) bullet.moduleCollision.Disable();
            else
            {   
                bullet.moduleCollision.Enable();
                bullet.moduleCollision.SetColliders(colliders);
            }
            
            bullet.self.SetParent(transform);
        }
        
        private void SetupLineRenderer(float width, float length)
        {
            lineRenderer.widthMultiplier = width;
            
            Vector3 position = myTransform.localPosition;
            lineRenderer.SetPosition(0, Vector2.zero);
            lineRenderer.SetPosition(1,Vector2.zero + Vector2.up * length);
        }

        private void SetupColliders(float width, float length)
        {
            float colliderOffset = width/2;
            for (int i = 0; i < 3; i++)
            {
                colliders[i].lineStart = new Vector2(colliderOffset * (i - 1), 0);
                colliders[i].lineEnd = new Vector2(colliderOffset * (i - 1), length);
            }
        }
        
    }
}