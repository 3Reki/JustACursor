using System.Collections;
using Player;
using UnityEngine;

namespace LD
{
    public class DamageArea : MonoBehaviour
    {
        private IEnumerator damageCoroutine;
        private Collider2D target;
        private PlayerCollision targetCollision;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var playerCollision = other.GetComponent<PlayerCollision>();
            if (!playerCollision) return;
            
            target = other;
            targetCollision = playerCollision;
            damageCoroutine = DamageCoroutine();
            StartCoroutine(damageCoroutine);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other == target)
            {
                StopCoroutine(damageCoroutine);
            }
        }

        private IEnumerator DamageCoroutine()
        {
            while (true)
            {
                yield return null;
                targetCollision.Damage();
            }
        }
    }
}