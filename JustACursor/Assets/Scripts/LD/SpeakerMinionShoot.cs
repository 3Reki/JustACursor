using System.Collections;
using System.Threading.Tasks;
using BulletPro;
using Unity.VisualScripting;
using UnityEngine;

namespace LD
{
    public class SpeakerMinionShoot : MonoBehaviour
    {
        [Header("Emitter")]
        [SerializeField] private BulletEmitter emitter;
        [SerializeField] private Transform firePosition;

        [Header("Time")]
        [SerializeField] private bool isLooping;
        [SerializeField] private float previewDuration;
        [SerializeField] private float laserDuration;
        [SerializeField] private float timeBeforeNextPreview;
        
        [Header("Preview")]
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private float laserWidth;
        [SerializeField] private float laserLength;
        [SerializeField, Range(0,1)] private float previewAlpha;

        private Gradient laserGradient;
        private Gradient previewGradient;

        private void OnEnable()
        {
            Energy.onGameSpeedUpdate += UpdateSpeed;
        }

        private void OnDisable()
        {
            Energy.onGameSpeedUpdate -= UpdateSpeed;
        }

        private void Awake()
        {
            lineRenderer.widthMultiplier = laserWidth;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0,firePosition.position);
            lineRenderer.SetPosition(1,firePosition.position+Vector3.up*laserLength);
            laserGradient = lineRenderer.colorGradient;
            previewGradient = new Gradient();
            previewGradient.SetKeys(laserGradient.colorKeys,
                new GradientAlphaKey[2]
            {
                new(previewAlpha, 0),
                new(previewAlpha, 1)
            });
            
            BulletParams myBullet = emitter.emitterProfile.GetBullet("Bullet");
            
            float colliderWidth = laserWidth / 20;
            
            myBullet.colliders[0].lineStart = new Vector2(-colliderWidth, 0);
            myBullet.colliders[0].lineEnd = new Vector2(-colliderWidth, laserLength);

            myBullet.colliders[1].lineEnd = new Vector2(0, laserLength);
            
            myBullet.colliders[2].lineStart = new Vector2(colliderWidth, 0);
            myBullet.colliders[2].lineEnd = new Vector2(colliderWidth, laserLength);

            myBullet.lifespan = new DynamicFloat(laserDuration);
            
            /*PatternParams myPattern = emitter.emitterProfile.GetPattern("Pattern");
            myPattern.instructionLists[0].instructions[2].waitTime = new DynamicFloat();*/

            StartCoroutine(LaserCycle());
        }

        private IEnumerator LaserCycle()
        {
            yield return new WaitForSeconds(timeBeforeNextPreview);
            
            lineRenderer.colorGradient = previewGradient;
            lineRenderer.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(previewDuration);
            
            emitter.Play();
            lineRenderer.colorGradient = laserGradient;
            
            yield return new WaitForSeconds(laserDuration);
            
            lineRenderer.gameObject.SetActive(false);
            emitter.Stop();

            StartCoroutine(LaserCycle());
        }
    
        private void UpdateSpeed()
        {
            //emitter.rootBullet.modulePatterns.patternRuntimeInfo[0].instructionLists[0].instructions[2].waitTime = timeBetweenLasers/Energy.GameSpeed;
        }
    }
}
