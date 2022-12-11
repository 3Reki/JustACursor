using System.Collections;
using UnityEngine;

namespace CameraScripts
{
    public class CameraController : MonoBehaviour
    {
        public static Camera mainCamera;
        public static CameraController instance;
        
        public Transform target;
    
        [SerializeField] private Vector3 localPositionToMove = new(0, 5, -15);
        [SerializeField] private Vector3 localPositionToLook = new(0, -1, 5);
        [SerializeField] private float movingSpeed = 0.02f;
        [SerializeField] private float rotationSpeed = 0.1f;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

        private void Start()
        {
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
        
        public static void ShakeCamera(float intensity, float timer)
        {
            instance.StartCoroutine(CameraShakeCoroutine(intensity, timer));
        }

        private static IEnumerator CameraShakeCoroutine(float intensity, float timer)
        {
            Vector3 lastCameraMovement = Vector3.zero;
            var transform1 = mainCamera.transform;
            Vector3 initialPosition = transform1.position;
            
            while (timer > 0f) {
                Vector3 randomMovement = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * intensity;
                transform1.position = transform1.position - lastCameraMovement + randomMovement;
                lastCameraMovement = randomMovement;
                yield return null;
                timer -= Time.unscaledDeltaTime;
            }

            transform1.position = initialPosition;
        }
    }
}
