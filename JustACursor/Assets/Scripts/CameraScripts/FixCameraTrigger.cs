using DG.Tweening;
using UnityEngine;

namespace CameraScripts
{
    public class FixCameraTrigger : MonoBehaviour
    {
        [SerializeField] private Vector2 anchorPosition;
        [SerializeField] private float movementDuration;
        [SerializeField] private float viewSize;

        private void OnTriggerEnter2D(Collider2D other)
        {
            CameraController.instance.enabled = false;
            CameraController.mainCamera.transform.DOMove(transform.TransformPoint(new Vector3(anchorPosition.x, anchorPosition.y, -10)), movementDuration);
            CameraController.mainCamera.DOOrthoSize(viewSize, movementDuration);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            /*var style = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fixedWidth = triggerBounds.size.x,
                normal = new GUIStyleState { textColor = Color.white }
            };
            
            //Handles.Label(position, "FixCameraTrigger", style);*/
        }

        private void OnDrawGizmosSelected()
        {
            Bounds triggerBounds = GetComponent<BoxCollider2D>().bounds;
            Vector3 position = triggerBounds.center;
            Vector3 size = triggerBounds.size;

            Gizmos.color = new Color(0, 0, 1);
            Gizmos.DrawWireCube(position, size);
            Gizmos.DrawLine(transform.position, transform.TransformPoint(anchorPosition));
            Gizmos.DrawWireSphere(transform.TransformPoint(anchorPosition), .5f);
            
            Gizmos.color = new Color(0, 0, .5f, .5f);
            Gizmos.DrawCube(position, size);
            Gizmos.DrawSphere(transform.TransformPoint(anchorPosition), .5f);
        }
#endif
    }
}
