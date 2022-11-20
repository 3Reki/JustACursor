using DG.Tweening;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
            Bounds triggerBounds = GetComponent<BoxCollider2D>().bounds;
            Vector3 position = triggerBounds.center;
            Vector3 size = triggerBounds.size;
            
            Gizmos.color = new Color(1, .4f, .4f);
            Gizmos.DrawWireCube(position, size);
            Gizmos.color = new Color(1, .15f, .15f, .7f);
            Gizmos.DrawCube(position, size);
            
            var style = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState {textColor = Color.white}
            };

            Handles.Label(position, "FixCameraTrigger", style);

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.TransformPoint(anchorPosition));
            Gizmos.DrawSphere(transform.TransformPoint(anchorPosition), 1f);
        }
#endif
    }
}
