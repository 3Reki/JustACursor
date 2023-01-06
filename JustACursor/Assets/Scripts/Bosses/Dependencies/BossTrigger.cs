using Player;
using UnityEngine;

namespace Bosses.Dependencies
{
    public class BossTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject bossToSpawn;
        [SerializeField] private GameObject invisibleWalls;

        private PlayerController player;
        private Boss boss;

        private void Awake() {
            boss = bossToSpawn.GetComponentInChildren<Boss>(true);
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.TryGetComponent(out PlayerController player)) {
                if (player.transform.position.y < transform.position.y) return;

                this.player = player;
                
                bossToSpawn.gameObject.SetActive(true);
                invisibleWalls.SetActive(true);
                enabled = false;
                
                player.Health.onDeath.AddListener(ResetTrigger);
            }
        }

        private void ResetTrigger() {
            boss.Reset();
            bossToSpawn.gameObject.SetActive(false);
            invisibleWalls.SetActive(false);
            enabled = true;
            
            player.Health.onDeath.RemoveListener(ResetTrigger);
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Bounds triggerBounds = GetComponent<BoxCollider2D>().bounds;
            Vector3 position = triggerBounds.center;
            Vector3 size = triggerBounds.size;
            
            Gizmos.color = new Color(1,1, 1, .5f);
            Gizmos.DrawCube(position, size);
            
            Gizmos.color = new Color(0, 0, 0);
            Gizmos.DrawWireCube(position, size);
        }
#endif
    }
}