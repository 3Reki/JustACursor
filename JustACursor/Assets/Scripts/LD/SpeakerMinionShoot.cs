using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace LD
{
    public class SpeakerMinionShoot : MonoBehaviour
    {
        public enum Movement { None, Horizontal, Vertical }

        [field:SerializeField] public bool IsActiveAtStart;
        
        [Header("Laser")]
        [SerializeField] private Laser laser;
        [SerializeField] private float timeBeforeFirstFire;
        [SerializeField] private float previewDuration;
        [SerializeField] private float laserDuration;
        [SerializeField] private float timeBeforeNextPreview;
        [SerializeField] private bool isEndless;
        [SerializeField] private bool hasCollision;

        [Header("Movement")]
        [SerializeField] private Movement movementAxis;
        [SerializeField] private AnimationCurve movementCurve;
        [SerializeField, Range(1,30)] private float amplitude;
        [SerializeField, Range(0.1f,20)] private float period;

        private Vector2 startPosition;
        private float curveTime;

        private void Awake()
        {
            if (!IsActiveAtStart) return;

            startPosition = transform.position;
            
            StartCoroutine(FirstFireDelay());
            if (movementAxis != Movement.None) StartCoroutine(MovementLoop());
        }

        private IEnumerator FirstFireDelay()
        {
            yield return new WaitForSeconds(timeBeforeFirstFire);
            StartCoroutine(isEndless ? ShootInfinite() : ShootLoop());
        }

        private IEnumerator ShootLoop()
        {
            yield return StartCoroutine(laser.Fire(previewDuration,laserDuration,hasCollision));
            yield return new WaitForSeconds(timeBeforeNextPreview/Energy.GameSpeed);
            StartCoroutine(ShootLoop());
        }

        private IEnumerator ShootInfinite()
        {
            yield return StartCoroutine(laser.Fire(0,float.PositiveInfinity,hasCollision));
        }
        
        public IEnumerator ShootOneShot(float previewDuration, float laserDuration, float laserWidth, float laserLength)
        {
            yield return StartCoroutine(laser.CustomFire(previewDuration, laserDuration, laserWidth, laserLength));
        }

        private IEnumerator MovementLoop()
        {
            float newPos = movementCurve.Evaluate(curveTime)*amplitude;
            
            if (movementAxis == Movement.Horizontal)
            {
                transform.DOComplete();
                transform.DOMoveX(startPosition.x+newPos, Time.deltaTime / period).SetEase(Ease.Linear);
            }
            else if (movementAxis == Movement.Vertical)
            {
                transform.DOComplete();
                transform.DOMoveY(startPosition.y+newPos, Time.deltaTime / period).SetEase(Ease.Linear);
            }
            
            curveTime += Time.deltaTime / period;
            curveTime %= 1;

            yield return new WaitForSeconds(Time.deltaTime / period);
            StartCoroutine(MovementLoop());
        }
    }
}
