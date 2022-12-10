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
            lineRenderer.widthMultiplier = laserWidth;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0,transform.position);
            lineRenderer.SetPosition(1,transform.position+Vector3.up*laserLength);
            
            InitNewColliders();
        }
        
        private void InitNewColliders()
        {
            float colliderOffset = laserWidth/2;
            
            newColliders[0] = new BulletCollider
            {
                colliderType = BulletColliderType.Line,
                lineStart = new Vector2(-colliderOffset, 0),
                lineEnd = new Vector2(-colliderOffset, laserLength)
            };
            
            newColliders[1] = new BulletCollider
            {
                colliderType = BulletColliderType.Line,
                lineEnd = new Vector2(0, laserLength)
            };
            
            newColliders[2] = new BulletCollider
            {
                colliderType = BulletColliderType.Line,
                lineStart = new Vector2(colliderOffset, 0),
                lineEnd = new Vector2(colliderOffset, laserLength)
            };
        }

        public void ShowPreview()
        {
            lineRenderer.gameObject.SetActive(true);
            lineRenderer.colorGradient = previewGradient;
        }

        public IEnumerator Fire(float laserDuration)
        {
            emitter.Play();
            yield return new WaitForSeconds(Time.deltaTime);
            if (emitter.bullets.Count > 0) SetupBullet(emitter.bullets[^1], laserDuration);

            lineRenderer.colorGradient = laserGradient;
            
            yield return new WaitForSeconds(laserDuration/Energy.GameSpeed);
            
            emitter.Stop();
            lineRenderer.gameObject.SetActive(false);
        }
        
        private void SetupBullet(BulletPro.Bullet bullet, float laserDuration)
        {
            bullet.moduleLifespan.lifespan = laserDuration/Energy.GameSpeed;
            bullet.moduleCollision.SetColliders(newColliders);
        }
    }
}