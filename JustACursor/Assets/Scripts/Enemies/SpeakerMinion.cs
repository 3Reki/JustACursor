using System.Collections;
using DG.Tweening;
using LD;
using UnityEngine;

namespace Enemies
{
    public class SpeakerMinion : MonoBehaviour, ILaserHolder
    {
        [SerializeField] private bool isActiveAtStart;

        [field: Header("Laser")]
        [SerializeField] private Laser laser;
        [SerializeField] private float laserWidth;
        [SerializeField] private float laserLength;
        [SerializeField] private float timeBeforeFirstFire;
        [SerializeField] private float previewDuration;
        [SerializeField] private float laserDuration;
        [SerializeField] private float laserCooldown;
        [SerializeField] private bool hasCollision;
        [SerializeField] private bool isEndless;

        [Header("Movement")]
        [SerializeField] private Movement movementAxis;
        [SerializeField] private AnimationCurve movementCurve;
        [SerializeField, Range(1,30)] private float amplitude;
        [SerializeField, Range(0.1f,20)] private float period;

        [Header("Editor Only")]
        [SerializeField] private bool showPreview;

        private Vector2 startPosition;
        private float curveTime;
        
        private void Awake()
        {
            if (!isActiveAtStart) return;

            startPosition = transform.position;

            if (isEndless)
                StartInfiniteFire();
            else
                StartCoroutine(FirstFireDelay());

            if (movementAxis != Movement.None) StartCoroutine(MovementLoop());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartFire(1,1,2,3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartFire(0.5f,0.5f,6,4);
            }
        }
        
        private IEnumerator FirstFireDelay()
        {
            yield return new WaitForSeconds(timeBeforeFirstFire);
            StartCoroutine(FireLoop());
        }
        
        public void StartFire(float previewDuration, float laserDuration, float laserWidth, float laserLength)
        {
            laser.StartFire(previewDuration, laserDuration, laserWidth, laserLength);
        }

        public void CeaseFire()
        {
            laser.StopFire();
        }
        
        private void StartDefaultFire()
        {
            laser.StartFire(previewDuration, laserDuration, laserWidth, laserLength, hasCollision);
        }

        private void StartInfiniteFire()
        {
            laser.StartFire(0, float.PositiveInfinity, laserWidth, laserLength, hasCollision);
        }

        private IEnumerator FireLoop()
        {
            StartDefaultFire();
            float timer = previewDuration + laserDuration;
            while (timer > 0)
            {
                yield return null;
                timer -= Time.deltaTime * Energy.GameSpeed;
            }
            
            CeaseFire();
            float cooldown = laserCooldown;
            while (cooldown > 0)
            {
                yield return null;
                cooldown -= Time.deltaTime * Energy.GameSpeed;
            }
            
            StartCoroutine(FireLoop());
        }
        
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
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
        
        private enum Movement { None, Horizontal, Vertical }

        private void OnDrawGizmos()
        {
            if (!showPreview) return;

            Transform laserTransform = laser.transform;
            
            Gizmos.matrix = laserTransform.localToWorldMatrix;

            Vector3 center = new(0, laserLength / 2);
            Vector3 size = new(laserWidth, laserLength);

            Gizmos.color = new Color(1, 0, 0, .5f);
            Gizmos.DrawCube(center,size);
        }
    }
}
