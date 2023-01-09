using UnityEngine;

namespace LD
{
    public class DialogueTrigger : MonoBehaviour
    {
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Bounds triggerBounds = GetComponent<BoxCollider2D>().bounds;
            Vector3 position = triggerBounds.center;
            Vector3 size = triggerBounds.size;
            
            Gizmos.color = new Color(0, 1, 1, .5f);
            Gizmos.DrawCube(position, size);
            
            Gizmos.color = new Color(.5f, 1, 1);
            Gizmos.DrawWireCube(position, size);
        }
#endif
    }
}