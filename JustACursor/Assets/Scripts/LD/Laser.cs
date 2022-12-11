using System.Collections;
using BulletPro;
using UnityEngine;

namespace LD
{
    public class Laser : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        
        [Header("Emitter")]
        [SerializeField] private BulletEmitter emitter;
        
        [Header("Preview")]
        [SerializeField] private Gradient previewGradient;
        
        [Header("Laser")]
        [SerializeField] private float laserWidth;
        [SerializeField] private float laserLength;
        [SerializeField] private Gradient laserGradient;
        
        private BulletCollider[] newColliders = new BulletCollider[3];
        
        private void Awake()
        {
            InitLineRenderer(laserWidth, laserLength);
            InitNewColliders(laserWidth, laserLength);
        }
        
        public IEnumerator Fire(float previewDuration, float laserDuration)
        {
            lineRenderer.gameObject.SetActive(true);
            lineRenderer.colorGradient = previewGradient;
            
            //Remove Time.deltaTime bc it's the time for the laser bullet to initialize
            yield return new WaitForSeconds(previewDuration/Energy.GameSpeed-Time.deltaTime);
            
            emitter.Play();
            //Wait for bullet to initialize
            yield return new WaitForSeconds(Time.deltaTime);
            if (emitter.bullets.Count > 0) SetupBullet(emitter.bullets[^1], laserDuration);

            lineRenderer.colorGradient = laserGradient;
            
            yield return new WaitForSeconds(laserDuration/Energy.GameSpeed);
            
            emitter.Stop();
            lineRenderer.gameObject.SetActive(false);
        }

        public IEnumerator CustomFire(float previewDuration, float laserDuration, float customWidth, float customLength)
        {
            InitLineRenderer(customWidth, customLength);
            InitNewColliders(customWidth, customLength);

            yield return StartCoroutine(Fire(previewDuration, laserDuration));

            InitLineRenderer(laserWidth, laserLength);
            InitNewColliders(laserWidth, laserLength);
        }
        
        private void SetupBullet(BulletPro.Bullet bullet, float laserDuration)
        {
            bullet.moduleLifespan.lifespan = laserDuration/Energy.GameSpeed;
            bullet.moduleCollision.SetColliders(newColliders);
        }

        private void InitLineRenderer(float width, float length)
        {
            lineRenderer.widthMultiplier = width;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0,transform.position);
            lineRenderer.SetPosition(1,transform.position+transform.up*length);
        }
        
        private void InitNewColliders(float width, float length)
        {
            float colliderOffset = width/2;
            
            newColliders[0] = new BulletCollider
            {
                colliderType = BulletColliderType.Line,
                lineStart = new Vector2(-colliderOffset, 0),
                lineEnd = new Vector2(-colliderOffset, length)
            };
            
            newColliders[1] = new BulletCollider
            {
                colliderType = BulletColliderType.Line,
                lineEnd = new Vector2(0, length)
            };
            
            newColliders[2] = new BulletCollider
            {
                colliderType = BulletColliderType.Line,
                lineStart = new Vector2(colliderOffset, 0),
                lineEnd = new Vector2(colliderOffset, length)
            };
        }
    }
}