using System;
using UnityEngine;

namespace CameraScripts
{
    public class CameraController : MonoBehaviour
    {
        public static Camera mainCamera;
        public static CameraController instance;
        
        public Transform target;
    
        [SerializeField] private Vector3 localPositionToMove = new Vector3(0, 5, -15);
        [SerializeField] private Vector3 localPositionToLook = new Vector3(0, -1, 5);
        [SerializeField] private float movingSpeed = 0.02f;
        [SerializeField] private float rotationSpeed = 0.1f;

        private Vector3 wantedPosition;
        private Quaternion wantedRotation;
        private Vector3 currentPosition;

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
            wantedPosition = target.TransformPoint(localPositionToMove);
            wantedPosition.y = target.position.y + localPositionToMove.y;
            currentPosition = transform.position;
            currentPosition = Vector3.Lerp(currentPosition, wantedPosition, movingSpeed);
            transform.position = currentPosition;

            wantedRotation = Quaternion.LookRotation(target.TransformPoint(localPositionToLook) - currentPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, rotationSpeed);
        }
    }
}
