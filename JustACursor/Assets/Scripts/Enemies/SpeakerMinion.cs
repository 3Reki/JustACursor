using System.Collections;
using LD;
using UnityEngine;

namespace Enemies
{
    public class SpeakerMinion : MonoBehaviour, ILaserHolder
    {
        [SerializeField] private bool IsActiveAtStart;

        [Header("Laser")]
        [SerializeField] private Laser laser;
        [SerializeField] private float laserWidth;
        [SerializeField] private float laserLength;
        [SerializeField] private float timeBeforeFirstFire;
        [SerializeField] private float previewDuration;
        [SerializeField] private float laserDuration;
        [SerializeField] private float laserCooldown;
        [SerializeField] private bool hasCollision;
        [SerializeField] private bool isEndless;

        [Header("Editor Preview")]
        [SerializeField] private bool showPreview;

        private void Start()
        {
            if (!IsActiveAtStart) return;
            
            if (isEndless)
                StartInfiniteFire();
            else
                StartCoroutine(FirstFireDelay());
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

        private void OnDrawGizmosSelected()
        {
            if (!showPreview) return;

            Transform laserTransform = laser.transform;
            
            Gizmos.matrix = laserTransform.localToWorldMatrix;

            Vector3 center = new(0, laserLength / 2);
            Vector3 size = new(laserWidth, laserLength);

            Gizmos.color = new Color(1, 1, 0, .3f);
            Gizmos.DrawCube(center,size);
        }
    }
}
