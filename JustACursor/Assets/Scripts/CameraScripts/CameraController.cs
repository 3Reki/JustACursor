using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace CameraScripts
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;
        
        [SerializeField] private Transform target;
    
        [SerializeField] private Vector3 localPositionToMove = new(0, 5, -15);
        [SerializeField] private Vector3 localPositionToLook = new(0, -1, 5);
        [SerializeField] private float movingSpeed = 0.02f;
        [SerializeField] private float rotationSpeed = 0.1f;

        private static Camera mainCamera;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            mainCamera = Camera.main;
        }
        
        private void FixedUpdate()
        {
            Vector3 wantedPosition = target.TransformPoint(localPositionToMove);
            wantedPosition.y = target.position.y + localPositionToMove.y;
            Vector3 currentPosition = transform.position;
            currentPosition = Vector3.Lerp(currentPosition, wantedPosition, movingSpeed);
            transform.position = currentPosition;

            Quaternion wantedRotation = Quaternion.LookRotation(target.TransformPoint(localPositionToLook) - currentPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, rotationSpeed);
        }

        public void Move(Vector3 position, float duration) {
            mainCamera.transform.DOKill();
            mainCamera.transform.DOMove(position, duration);
        }

        public void SetViewSize(float viewSize, float duration) {
            mainCamera.DOKill();
            mainCamera.DOOrthoSize(viewSize, duration);
        }

        public static void TeleportToTarget()
        {
            Instance.transform.position = Instance.target.position + Instance.localPositionToMove;
        } 
        
        public static void ShakeCamera(float intensity, float timer)
        {
            Instance.StartCoroutine(CameraShakeCoroutine(intensity, timer));
        }

        private static IEnumerator CameraShakeCoroutine(float intensity, float timer)
        {
            Vector3 lastCameraMovement = Vector3.zero;
            Transform camTransform = mainCamera.transform;
            Vector3 initialPosition = camTransform.position;
            
            while (timer > 0f) {
                Vector3 randomMovement = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * intensity;
                camTransform.position = camTransform.position - lastCameraMovement + randomMovement;
                lastCameraMovement = randomMovement;
                yield return null;
                timer -= Time.unscaledDeltaTime;
            }

            camTransform.position = initialPosition;
        }
    }
}
