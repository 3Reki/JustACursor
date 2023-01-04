using System.Collections;
using BulletPro;
using Player;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Tilemaps;

namespace LD
{
    public class Laser : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private BulletEmitter emitter;

        [Header("Render")]
        [SerializeField] private Gradient previewGradient;
        [SerializeField] private Gradient laserGradient;
        [SerializeField] private ParticleSystem ps_Laser;

        private readonly BulletCollider[] colliders = new BulletCollider[5];
        private IEnumerator fireEnumerator;
        private LayerMask layerMask;

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

            layerMask.value = LayerMask.GetMask("Default");
        }

        public void StartFire(float previewDuration, float laserDuration, float laserWidth, float laserLength, bool hasCollision = true) {
            laserLength = GetCorrectLaserLength(laserLength, laserWidth);

            SetupLineRenderer(laserWidth, laserLength);
            SetupColliders(laserWidth, laserLength);

            fireEnumerator = Fire(previewDuration, laserDuration, hasCollision);
            StartCoroutine(fireEnumerator);
        }

        public void StopFire()
        {
            if (fireEnumerator != null)
                StopCoroutine(fireEnumerator);

            Clear();
        }

        private void ShowPreview()
        {
            lineRenderer.gameObject.SetActive(true);
            lineRenderer.colorGradient = previewGradient;
        }

        private void Clear() {
            emitter.Stop();
            emitter.Kill();
            
            lineRenderer.gameObject.SetActive(false);
            ps_Laser.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
            ps_Laser.gameObject.SetActive(false);
        }

        private IEnumerator Fire(float previewDuration, float laserDuration, bool hasCollision)
        {
            ShowPreview();

            //Remove Time.deltaTime bc it's the time for the laser bullet to initialize
            while (previewDuration > 0)
            {
                yield return null;
                previewDuration -= Time.deltaTime * Energy.GameSpeed;
            }
            
            emitter.Play();
            //Wait for bullet to initialize
            while (true)
            {
                yield return null;
                if (emitter.bullets.Count > 0)
                {
                    SetupBullet(emitter.bullets[^1], hasCollision);
                    break;
                }
            }

            lineRenderer.colorGradient = laserGradient;
            ps_Laser.Play();
            ps_Laser.gameObject.SetActive(true);

            laserDuration -= Time.deltaTime * Energy.GameSpeed;
            while (laserDuration > 0)
            {
                yield return null;
                laserDuration -= Time.deltaTime * Energy.GameSpeed;
            }

            Clear();
        }

        private float GetCorrectLaserLength(float customLength, float customWidth) {
            float rayLength = customLength + customWidth / 2;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, rayLength, layerMask);
            
            if (hit) return hit.distance - customWidth / 2;
            return customLength;
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

            lineRenderer.SetPosition(0, Vector2.zero);
            lineRenderer.SetPosition(1, Vector2.zero + Vector2.up * length);
        }

        private void SetupColliders(float width, float length)
        {
            float colliderOffset = width / (colliders.Length-1);
            for (int i = 0; i < colliders.Length; i++) {
                colliders[i].lineStart = new Vector2(colliderOffset * (i-2), 0);
                colliders[i].lineEnd = new Vector2(colliderOffset * (i-2), length);
            }
            
            /*colliders[0].lineStart = new Vector2(-width / 2, 0);
            colliders[0].lineEnd = new Vector2(-width / 2, length);
            
            colliders[1].lineStart = new Vector2(-width / 2, length);
            colliders[1].lineEnd = new Vector2(width / 2, length);
            
            colliders[2].lineStart = new Vector2(width / 2, length);
            colliders[2].lineEnd = new Vector2(width / 2, 0);
            
            colliders[3].lineStart = new Vector2(width / 2, 0);
            colliders[3].lineEnd = new Vector2(-width / 2, 0);*/
        }
    }
}