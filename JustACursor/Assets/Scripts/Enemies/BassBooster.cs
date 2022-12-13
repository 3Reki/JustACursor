using System;
using System.Collections;
using BulletPro;
using UnityEngine;

namespace Enemies
{
    public class BassBooster : MonoBehaviour
    {
        [SerializeField] private BulletEmitter emitter;
        
        [SerializeField] private float timeBeforeFirstFire;
        [SerializeField] private float shockwaveLifespan;
        [SerializeField] private Color shockwaveColor;
        [SerializeField] private AnimationCurve shockwaveScaleOverLifetime;
        [SerializeField] private float shockwaveCooldown;

        private void OnEnable()
        {
            Energy.onGameSpeedUpdate += UpdateLifespan;
        }

        private void OnDisable()
        {
            Energy.onGameSpeedUpdate -= UpdateLifespan;
        }

        private void Awake()
        {
            StartCoroutine(FirstFireDelay());
        }

        private IEnumerator FirstFireDelay()
        {
            yield return new WaitForSeconds(timeBeforeFirstFire);
            StartCoroutine(FireLoop());
        }
        
        private IEnumerator FireLoop()
        {
            emitter.Play();
            //Wait for bullet to initialize
            while (true)
            {
                yield return null;
                if (emitter.bullets.Count > 0)
                {
                    SetupBullet(emitter.bullets[^1]);
                    break;
                }
            }
            
            emitter.Stop();
            
            float cooldown = shockwaveCooldown;
            while (cooldown > 0)
            {
                yield return null;
                cooldown -= Time.deltaTime * Energy.GameSpeed;
            }
            
            StartCoroutine(FireLoop());
        }
        
        private void SetupBullet(BulletPro.Bullet bullet)
        {
            bullet.moduleLifespan.lifespan = shockwaveLifespan;
            bullet.moduleMovement.scaleOverLifetime.curve = shockwaveScaleOverLifetime;
            bullet.moduleRenderer.startColor = shockwaveColor;

            bullet.self.SetParent(transform);
        }

        private void UpdateLifespan()
        {
            /*foreach (BulletPro.Bullet bullet in emitter.bullets)
            {
                bullet.moduleLifespan.GetRemainingLifespan()
                bullet.moduleLifespan.lifespan = bullet.timeSinceAlive+(shockwaveLifespan-bullet.timeSinceAlive)*Energy.GameSpeed;
            }*/
        }
    }
}