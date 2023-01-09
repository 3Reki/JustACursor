using System.Collections;
using BulletPro;
using Player;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Tilemaps;

namespace LD
{
    public class Laser : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private BoxCollider2D laserCollider;

        [Header("Render")]
        [SerializeField] private Gradient previewGradient;
        [SerializeField] private Gradient laserGradient;
        [SerializeField] private ParticleSystem psLaser;
        
        private IEnumerator fireEnumerator;
        private LayerMask layerMask;

        private void Awake()
        {
            lineRenderer.positionCount = 2;

            layerMask.value = LayerMask.GetMask("Wall");
        }

        public void StartFire(float previewDuration, float laserDuration, float laserWidth, float laserLength, bool hasCollision = true) {
            laserLength = GetCorrectLaserLength(laserLength, laserWidth);

            SetupLineRenderer(laserWidth, laserLength);
            SetupCollider(laserWidth,laserLength);

            fireEnumerator = Fire(previewDuration, laserDuration, hasCollision);
            StartCoroutine(fireEnumerator);
        }

        public void StopFire()
        {
            if (fireEnumerator != null)
                StopCoroutine(fireEnumerator);

            Clear();
        }

        private void ShowPreview()
        {
            lineRenderer.gameObject.SetActive(true);
            lineRenderer.colorGradient = previewGradient;
        }

        private void Active(bool hasCollision)
        {
            if (hasCollision) laserCollider.enabled = true;
            lineRenderer.colorGradient = laserGradient;
            psLaser.Play();
        }

        private void Clear()
        {
            laserCollider.enabled = false;
            lineRenderer.gameObject.SetActive(false);
            psLaser.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        private IEnumerator Fire(float previewDuration, float laserDuration, bool hasCollision)
        {
            ShowPreview();

            while (previewDuration > 0)
            {
                yield return null;
                previewDuration -= Time.deltaTime * Energy.GameSpeed;
            }

            Active(hasCollision);

            while (laserDuration > 0)
            {
                yield return null;
                laserDuration -= Time.deltaTime * Energy.GameSpeed;
            }

            Clear();
        }

        private float GetCorrectLaserLength(float customLength, float customWidth) {
            float rayLength = customLength + customWidth / 2;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, rayLength, layerMask);
            
            if (hit) return hit.distance - customWidth / 2;
            return customLength;
        }

        private void SetupLineRenderer(float width, float length)
        {
            lineRenderer.widthMultiplier = width;

            lineRenderer.SetPosition(0, Vector2.zero);
            lineRenderer.SetPosition(1, Vector2.zero + Vector2.up * length);
        }

        private void SetupCollider(float width, float length)
        {
            laserCollider.size = new Vector2(width, length);
            laserCollider.offset = new Vector2(0, length / 2);
        }
    }
}