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
            CameraController.mainCamera.transform.DOMove(new Vector3(anchorPosition.x, anchorPosition.y, -10), movementDuration);
            CameraController.mainCamera.DOOrthoSize(viewSize, movementDuration);
        }
    }
}
