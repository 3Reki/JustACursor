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
            if (shootAtStart) Shoot();
        }

        public void Shoot()
        {
            StartCoroutine(LaserCycle());
        }

        private IEnumerator LaserCycle()
        {
            laser.ShowPreview();
            
            yield return new WaitForSeconds(previewDuration/Energy.GameSpeed-Time.deltaTime);

            yield return StartCoroutine(laser.Fire(laserDuration));
            
            yield return new WaitForSeconds(timeBeforeNextPreview/Energy.GameSpeed);

            if (isLooping) StartCoroutine(LaserCycle());
        }

        
    }
}
