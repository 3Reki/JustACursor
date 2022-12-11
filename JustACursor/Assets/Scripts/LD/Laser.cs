using System;
using System.Collections;
using System.Collections.Generic;
using BulletPro;
using UnityEngine;

namespace LD
{
    public class Laser : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        
        [Header("Emitter")]
        [SerializeField] private BulletEmitter emitter;
        
        [Header("Preview")]
        [SerializeField] private Gradient previewGradient;
        
        [Header("Laser")]
        [SerializeField] private float laserWidth;
        [SerializeField] private float laserLength;
        [SerializeField] private Gradient laserGradient;

        private int currentCollidersIndex;
        private List<BulletCollider[]> collidersList;
        private readonly BulletCollider[] baseColliders = new BulletCollider[3];
        private readonly BulletCollider[] customColliders = new BulletCollider[3];
        
        private void Awake()
        {
            lineRenderer.positionCount = 2;
            lineRenderer.widthMultiplier = laserWidth;
            lineRenderer.SetPosition(0,transform.localPosition);
            lineRenderer.SetPosition(1,transform.localPosition+transform.up*laserLength);
            UpdateColliders(baseColliders, laserWidth, laserLength);
            
            currentCollidersIndex = 0;
            collidersList = new List<BulletCollider[]>{ baseColliders, customColliders };
        }
        
        public IEnumerator Fire(float previewDuration, float laserDuration, bool hasCollision = true)
        {
            lineRenderer.gameObject.SetActive(true);
            lineRenderer.colorGradient = previewGradient;
            
            //Remove Time.deltaTime bc it's the time for the laser bullet to initialize
            yield return new WaitForSeconds(previewDuration/Energy.GameSpeed-Time.deltaTime);
            
            emitter.Play();
            //Wait for bullet to initialize
            yield return new WaitForSeconds(Time.deltaTime);
            if (emitter.bullets.Count > 0)
            {
                SetupBullet(emitter.bullets[^1], laserDuration, hasCollision ? collidersList[currentCollidersIndex] : null);
            }

            lineRenderer.colorGradient = laserGradient;
            
            yield return new WaitForSeconds(laserDuration/Energy.GameSpeed);
            
            emitter.Stop();
            lineRenderer.gameObject.SetActive(false);
        }

        public IEnumerator CustomFire(float previewDuration, float laserDuration, float customWidth, float customLength)
        {
            UpdateColliders(customColliders, customWidth, customLength);

            currentCollidersIndex = 1;
            yield return StartCoroutine(Fire(previewDuration, laserDuration));
            currentCollidersIndex = 0;
        }
        
        private void SetupBullet(BulletPro.Bullet bullet, float laserDuration, BulletCollider[] colliders)
        {
            //Lifespan Module
            if (float.IsPositiveInfinity(laserDuration)) bullet.moduleLifespan.hasLimitedLifetime = false;
            else bullet.moduleLifespan.lifespan = laserDuration/Energy.GameSpeed;
            
            //Collision Module
            if (colliders == null) bullet.moduleCollision.Disable();
            else bullet.moduleCollision.SetColliders(colliders);
            
            bullet.self.SetParent(transform);
        }
        
        private void UpdateColliders(BulletCollider[] colliders, float width, float length)
        {
            float colliderOffset = width/2;

            colliders[0] = new BulletCollider
            {
                colliderType = BulletColliderType.Line,
                lineStart = new Vector2(-colliderOffset, 0),
                lineEnd = new Vector2(-colliderOffset, length)
            };
            
            colliders[1] = new BulletCollider
            {
                colliderType = BulletColliderType.Line,
                lineEnd = new Vector2(0, length)
            };
            
            colliders[2] = new BulletCollider
            {
                colliderType = BulletColliderType.Line,
                lineStart = new Vector2(colliderOffset, 0),
                lineEnd = new Vector2(colliderOffset, length)
            };
        }
    }
}