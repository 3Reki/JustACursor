using System.Collections;
using Bosses.Instructions.Patterns;
using LD;
using UnityEngine;

namespace Enemies
{
    public class SpeakerMinion : MonoBehaviour, ILaserHolder
    {
        [Header("LD ONLY")]
        [SerializeField] private bool isFromLD = true;
        
        [Header("Parameters")]
        [SerializeField] private Transform myTransform;
        [SerializeField] private Laser laser;
        [SerializeField] private Pat_Laser laserPattern;
        [SerializeField] private float laserCooldown = 1;

        private void Start()
        {
            if (isFromLD) StartCoroutine(ShootLoop());
        }

        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            myTransform.SetPositionAndRotation(position, rotation);
        }

        public void StartFire(float previewDuration, float laserDuration, float laserWidth, float laserLength)
        {
            laser.StartFire(previewDuration, laserDuration, laserWidth, laserLength);
        }

        public void CeaseFire()
        {
            laser.StopFire();
        }
        
        private IEnumerator ShootLoop()
        {
            while (isFromLD)
            {
                laserPattern.Play(this);
                while (!laserPattern.isFinished)
                {
                    laserPattern.Update();
                    yield return null;
                }
            
                laserPattern.Stop();
                float cooldown = laserCooldown;
                while (cooldown > 0)
                {
                    yield return null;
                    cooldown -= Time.deltaTime * Energy.GameSpeed;
                }
            }
        }
        
    }
}
