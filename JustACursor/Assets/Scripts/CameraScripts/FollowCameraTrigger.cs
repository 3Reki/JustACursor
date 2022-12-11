using DG.Tweening;
using UnityEngine;

namespace CameraScripts
{
    public class FollowCameraTrigger : MonoBehaviour
    {
        [SerializeField] private float movementDuration;
        [SerializeField] private float viewSize;

        private void OnTriggerEnter2D(Collider2D other)
        {
            //TODO : Smooth transition FixCamera --> FollowCamera
            CameraController.instance.enabled = true;
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
                normal = new GUIStyleState {textColor = Color.white}
            };

            Handles.Label(position, "FollowCameraTrigger", style);*/
        }

        private void OnDrawGizmosSelected()
        {
            Bounds triggerBounds = GetComponent<BoxCollider2D>().bounds;
            Vector3 position = triggerBounds.center;
            Vector3 size = triggerBounds.size;
            
            Gizmos.color = new Color(1, 0, 0);
            Gizmos.DrawWireCube(position, size);
            Gizmos.color = new Color(.5f, 0, 0, .5f);
            Gizmos.DrawCube(position, size);
        }
#endif
    }
}
