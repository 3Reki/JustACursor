using Player;
using UnityEngine;

namespace LD
{
    public class Checkpoint : MonoBehaviour
    {
        public delegate void OnCheckpointEnter(Checkpoint checkpoint);
        public static OnCheckpointEnter onCheckpointEnter;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerController player))
            {
                onCheckpointEnter.Invoke(this);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Bounds triggerBounds = GetComponent<BoxCollider2D>().bounds;
            Vector3 position = triggerBounds.center;
            Vector3 size = triggerBounds.size;
            
            Gizmos.color = new Color(1, 1, 0, .5f);
            Gizmos.DrawCube(position, size);
            
            Gizmos.color = new Color(1, .5f, 0);
            Gizmos.DrawWireCube(position, size);
        }
#endif
    }
}