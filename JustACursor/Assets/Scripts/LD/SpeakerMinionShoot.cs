using System.Collections;
using BulletPro;
using UnityEngine;

namespace LD
{
    public class SpeakerMinionShoot : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        
        [Header("Emitter")]
        [SerializeField] private BulletEmitter emitter;
        [SerializeField] private Transform firePosition;

        [Header("Preview")]
        [SerializeField] private float previewDuration;
        [SerializeField] private float timeBeforeNextPreview;
        [SerializeField] private Gradient previewGradient;

        [Header("Laser")]
        [SerializeField] private bool isLooping;
        [SerializeField] private float laserWidth;
        [SerializeField] private float laserLength;
        [SerializeField] private Gradient laserGradient;

        [SerializeField] private float laserDuration;
        
        private BulletCollider[] newColliders = new BulletCollider[3];

        private void Awake()
        {
            lineRenderer.widthMultiplier = laserWidth;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0,firePosition.position);
            lineRenderer.SetPosition(1,firePosition.position+Vector3.up*laserLength);

            InitNewColliders();
        }

        private void Start()
        {
            StartCoroutine(LaserCycle());
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

        private IEnumerator LaserCycle()
        {
            yield return new WaitForSeconds(timeBeforeNextPreview/Energy.GameSpeed);
            
            lineRenderer.colorGradient = previewGradient;
            lineRenderer.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(previewDuration/Energy.GameSpeed-Time.deltaTime);
            
            emitter.Play();
            yield return new WaitForSeconds(Time.deltaTime);
            SetupBullet(emitter.bullets[^1]);
            
            lineRenderer.colorGradient = laserGradient;
            
            yield return new WaitForSeconds(laserDuration/Energy.GameSpeed);
            
            lineRenderer.gameObject.SetActive(false);
            emitter.Stop();

            StartCoroutine(LaserCycle());
        }

        private void SetupBullet(BulletPro.Bullet bullet)
        {
            bullet.moduleLifespan.lifespan = laserDuration/Energy.GameSpeed;
            bullet.moduleCollision.SetColliders(newColliders);
        }
    }
}
