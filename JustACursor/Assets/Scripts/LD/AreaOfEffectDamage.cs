using System.Collections;
using Player;
using UnityEngine;

namespace LD
{
    public class AreaOfEffectDamage : MonoBehaviour
    {
        private Coroutine damageCoroutine;
        private Collider2D target;

        private void OnDisable()
        {
            if (damageCoroutine != null) 
                StopCoroutine(damageCoroutine);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerCollision player))
            {
                damageCoroutine = StartCoroutine(DamageCR(player));
                target = other;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other != target) return;

            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
            }
                
        }

        private IEnumerator DamageCR(PlayerCollision player)
        {
            while (true)
            {
                yield return null;
                player.Damage();
            }
        }
    }
}
