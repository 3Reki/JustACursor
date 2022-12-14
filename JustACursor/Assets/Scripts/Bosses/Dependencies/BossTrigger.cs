using UnityEngine;

namespace Bosses.Dependencies
{
    public class BossTrigger : MonoBehaviour
    {
        [SerializeField] private Boss bossToSpawn;
        [SerializeField] private GameObject invisibleWalls;

        private void OnTriggerEnter2D(Collider2D other)
        {
            bossToSpawn.gameObject.SetActive(true);
            invisibleWalls.SetActive(true);
            enabled = false;
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