using System.Collections;
using Player;
using UnityEngine;

namespace LD
{
    public class AreaOfEffect : MonoBehaviour
    {
        [SerializeField] private GameObject preview;
        [SerializeField] private GameObject aoe;

        private Coroutine fireCoroutine;

        public void SetRadius(float radius)
        {
            transform.localScale = new Vector3(radius*2,radius*2,radius*2);
        }
        
        public void StartFire(float previewDuration, float aoeDuration, Vector3 spawnPosition = default)
        {
            transform.localPosition = spawnPosition;
            fireCoroutine = StartCoroutine(Fire(previewDuration, aoeDuration));
        }

        private IEnumerator Fire(float previewDuration, float aoeDuration)
        {
            preview.SetActive(true);
            
            float timer = previewDuration;
            while (timer > 0)
            {
                yield return null;
                timer -= Time.deltaTime * Energy.GameSpeed;
            }
            
            preview.SetActive(false);
            aoe.SetActive(true);

            timer = aoeDuration;
            while (timer > 0)
            {
                yield return null;
                timer -= Time.deltaTime * Energy.GameSpeed;
            }
            
            aoe.SetActive(false);
        }

        public void StopFire()
        {
            if (fireCoroutine != null)
                StopCoroutine(fireCoroutine);
        }
    }
}