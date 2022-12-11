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

            Camera cam = Camera.main;
            Vector3 camPos = transform.TransformPoint(anchorPosition);
            float height = 2f * viewSize;
            float width = height * cam.aspect;
            
            Gizmos.color = new Color(0, .5f, 1);
            Gizmos.DrawCube(position, size);
            
            Gizmos.color = new Color(0, 0, 1f, .5f);
            Gizmos.DrawWireCube(position, size);
            Gizmos.DrawLine(transform.position,camPos);
            Gizmos.DrawWireCube(camPos, new Vector2(width, height));
            Gizmos.DrawLine(new Vector3(camPos.x-width/2,camPos.y+height/2), new Vector3(camPos.x+width/2,camPos.y-height/2));
            Gizmos.DrawLine(new Vector3(camPos.x-width/2,camPos.y-height/2), new Vector3(camPos.x+width/2,camPos.y+height/2));
        }
#endif
    }
}
