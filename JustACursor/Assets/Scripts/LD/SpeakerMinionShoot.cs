using System.Collections;
using UnityEngine;

namespace LD
{
    public class SpeakerMinionShoot : MonoBehaviour
    {
        [SerializeField] private Laser laser;
        [SerializeField] private float timeBeforeNextPreview;
        [SerializeField] private float previewDuration;
        [SerializeField] private float laserDuration;
        [SerializeField] private bool shootAtStart;
        [SerializeField] private bool isLooping;

        private void Start()
        {
            if (shootAtStart) StartCoroutine(ShootLoop());
        }

        private IEnumerator ShootLoop()
        {
            yield return StartCoroutine(laser.Fire(previewDuration,laserDuration));
            yield return new WaitForSeconds(timeBeforeNextPreview/Energy.GameSpeed);
            if (isLooping) StartCoroutine(ShootLoop());
        }
        
        public IEnumerator ShootOneShot(float previewDuration, float laserDuration, float laserWidth, float laserLength)
        {
            yield return StartCoroutine(laser.CustomFire(previewDuration, laserDuration, laserWidth, laserLength));
        }
    }
}
