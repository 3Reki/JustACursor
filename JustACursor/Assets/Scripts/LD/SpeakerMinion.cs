using System.Collections;
using UnityEngine;

namespace LD
{
    public class SpeakerMinion : MonoBehaviour
    {
        [Header("LD ONLY")]
        [SerializeField] private bool isFromLD = true;
        
        [Header("Parameters")]
        [SerializeField] private Transform myTransform;
        [SerializeField] private Laser laser;
        [SerializeField] private float timeBeforeNextPreview;
        [SerializeField] private float previewDuration;
        [SerializeField] private float laserDuration;

        private void Start()
        {
            if (isFromLD) StartCoroutine(ShootLoop());
        }

        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            myTransform.SetPositionAndRotation(position, rotation);
        }

        public IEnumerator ShootOneShot(float previewDuration, float laserDuration, float laserWidth, float laserLength)
        {
            yield return laser.CustomFire(previewDuration, laserDuration, laserWidth, laserLength);
        }
        
        private IEnumerator ShootLoop()
        {
            yield return StartCoroutine(laser.Fire(previewDuration,laserDuration));
            yield return new WaitForSeconds(timeBeforeNextPreview/Energy.GameSpeed);
            if (isFromLD) StartCoroutine(ShootLoop());
        }
        
    }
}
