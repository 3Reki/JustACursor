using UnityEngine;

namespace CameraScripts
{
    public class FollowCameraTrigger : MonoBehaviour
    {
        [SerializeField] private float viewSize;
        [SerializeField] private float movementDuration;

        private void OnTriggerEnter2D(Collider2D other)
        {
            //TODO : Smooth transition FixCamera --> FollowCamera
            CameraController.Instance.enabled = true;
            CameraController.Instance.SetViewSize(viewSize,movementDuration);
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            /*var style = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fixedWidth = triggerBounds.size.x,
                normal = new GUIStyleState {textColor = Color.white}
            };

            Handles.Label(position, "FollowCameraTrigger", style);*/
        }

        private void OnDrawGizmosSelected()
        {
            Bounds triggerBounds = GetComponent<BoxCollider2D>().bounds;
            Vector3 position = triggerBounds.center;
            Vector3 size = triggerBounds.size;
            
            Camera cam = Camera.main;
            Vector2 camPos = transform.position;
            float height = 2f * viewSize;
            float width = height * cam.aspect;
            
            Gizmos.color = new Color(1, 0, 0, .5f);
            Gizmos.DrawCube(position, size);

            Gizmos.color = new Color(1f, .5f, .5f);
            Gizmos.DrawWireCube(position, size);
            Gizmos.DrawWireCube(camPos, new Vector2(width, height));
            Gizmos.DrawLine(new Vector3(camPos.x-width/2,camPos.y+height/2), new Vector3(camPos.x+width/2,camPos.y-height/2));
            Gizmos.DrawLine(new Vector3(camPos.x-width/2,camPos.y-height/2), new Vector3(camPos.x+width/2,camPos.y+height/2));
        }
#endif
    }
}
