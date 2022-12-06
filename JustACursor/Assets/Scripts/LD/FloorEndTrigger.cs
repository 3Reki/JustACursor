using Player;
using UnityEngine;

namespace LD
{
    public class FloorEndTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerController player))
            {
                LevelHandler.onEndFloor.Invoke();
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Bounds triggerBounds = GetComponent<BoxCollider2D>().bounds;
            Vector3 position = triggerBounds.center;
            Vector3 size = triggerBounds.size;
            
            Gizmos.color = new Color(0, 1, 0);
            Gizmos.DrawWireCube(position, size);
            Gizmos.color = new Color(0, .5f, 0, .5f);
            Gizmos.DrawCube(position, size);
        }
#endif
    }
}