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
    }
}
