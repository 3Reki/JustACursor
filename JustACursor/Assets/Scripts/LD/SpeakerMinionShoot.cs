using System.Collections;
using UnityEngine;

namespace LD
{
    public class SpeakerMinionShoot : MonoBehaviour
    {
        [Header("LD ONLY")]
        [SerializeField] private bool isFromLD = true;
        
        [Header("Parameters")]
        [SerializeField] private Laser laser;
        [SerializeField] private float timeBeforeNextPreview;
        [SerializeField] private float previewDuration;
        [SerializeField] private float laserDuration;

        private void Start()
        {
            if (isFromLD) StartCoroutine(ShootLoop());
        }

        private IEnumerator ShootLoop()
        {
            yield return StartCoroutine(laser.Fire(previewDuration,laserDuration));
            yield return new WaitForSeconds(timeBeforeNextPreview/Energy.GameSpeed);
            if (isFromLD) StartCoroutine(ShootLoop());
        }
        
        public IEnumerator ShootOneShot(float previewDuration, float laserDuration, float laserWidth, float laserLength)
        {
            yield return StartCoroutine(laser.CustomFire(previewDuration, laserDuration, laserWidth, laserLength));
        }
    }
}
