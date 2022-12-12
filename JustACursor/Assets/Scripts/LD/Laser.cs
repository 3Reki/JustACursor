using System.Collections;
using BulletPro;
using UnityEngine;

namespace LD
{
    public class Laser : MonoBehaviour
    {
        [SerializeField] private Transform myTransform;
        [SerializeField] private LineRenderer lineRenderer;
        
        [Header("Emitter")]
        [SerializeField] private BulletEmitter emitter;
        
        [Header("Preview")]
        [SerializeField] private Gradient previewGradient;
        
        [Header("Laser")]
        [SerializeField] private Gradient laserGradient;
        
        private readonly BulletCollider[] colliders = new BulletCollider[3];
        private IEnumerator fireEnumerator;

        private void Awake()
        {
            lineRenderer.positionCount = 2;
            InitColliders();
        }

        public void StartFire(float previewDuration, float laserDuration, float customWidth, float customLength)
        {
            SetupLineRenderer(customWidth, customLength);
            SetupColliders(customWidth, customLength);

            fireEnumerator = Fire(previewDuration, laserDuration);
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
            
            lineRenderer.gameObject.SetActive(false);
        }
        
        private IEnumerator Fire(float previewDuration, float laserDuration)
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
            if (emitter.bullets.Count > 0) emitter.bullets[^1].moduleCollision.SetColliders(colliders);;

            lineRenderer.colorGradient = laserGradient;
            
            laserDuration -= Time.deltaTime * Energy.GameSpeed;
            while (laserDuration > 0)
            {
                yield return null;
                laserDuration -= Time.deltaTime * Energy.GameSpeed;
            }
            
            emitter.Stop();
            lineRenderer.gameObject.SetActive(false);
        }

        private void SetupLineRenderer(float width, float length)
        {
            lineRenderer.widthMultiplier = width;

            Vector3 position = myTransform.position;
            lineRenderer.SetPosition(0, position);
            lineRenderer.SetPosition(1,position + myTransform.up * length);
        }
        
        private void SetupColliders(float width, float length)
        {
            float colliderOffset = width/2;

            for (int i = 0; i < 2; i++)
            {
                colliders[i].lineStart = new Vector2(colliderOffset * (i - 1), 0);
                colliders[i].lineEnd = new Vector2(colliderOffset * (i - 1), length);
            }
        }
        
        private void InitColliders()
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i] = new BulletCollider
                {
                    colliderType = BulletColliderType.Line
                };
            }
        }
    }
}