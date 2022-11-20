using DG.Tweening;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
            Bounds triggerBounds = GetComponent<BoxCollider2D>().bounds;
            Vector3 position = triggerBounds.center;
            Vector3 size = triggerBounds.size;
            
            Gizmos.color = new Color(1, .4f, .4f);
            Gizmos.DrawWireCube(position + new Vector3(0, 0, -1), size);
            Gizmos.color = new Color(1, .15f, .15f, .7f);
            Gizmos.DrawCube(position, size);
            
            var style = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState {textColor = Color.white}
            };

            Handles.Label(position + new Vector3(0, 0, -2), "FollowCameraTrigger", style);

        }
#endif
    }
}
