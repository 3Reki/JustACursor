using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour {
    
        private Camera camera;

        [Header("Action Keys")]
        [SerializeField] private KeyCode dashKey;
        [SerializeField] private KeyCode shootKey;
        [SerializeField] private KeyCode speedUpKey;
        [SerializeField] private KeyCode slowDownKey;

        private HashSet<InputAction> requestedActions = new HashSet<InputAction>();

        void Start() {
            camera = Camera.main;
        }
    
        private void Update() {
            CaptureInput();
        }
    
        private void CaptureInput() {
            if (Input.GetKeyDown(dashKey)) requestedActions.Add(InputAction.Dash);
            else if (Input.GetKeyUp(dashKey)) requestedActions.Remove(InputAction.Dash);

            if (Input.GetKeyDown(shootKey)) requestedActions.Add(InputAction.Shoot);
            else if (Input.GetKeyUp(shootKey)) requestedActions.Remove(InputAction.Shoot);
        
            if (Input.GetKeyDown(speedUpKey)) requestedActions.Add(InputAction.SpeedUp);
            else if (Input.GetKeyUp(speedUpKey)) requestedActions.Remove(InputAction.SpeedUp);
        
            if (Input.GetKeyDown(slowDownKey)) requestedActions.Add(InputAction.SlowDown);
            else if (Input.GetKeyUp(slowDownKey)) requestedActions.Remove(InputAction.SlowDown);
        }
    
        public float GetAxisRaw(Axis axis) {
            return Input.GetAxisRaw(axis == Axis.X ? "Horizontal" : "Vertical");
        }

        public Vector2 GetMousePos() {
            return camera.ScreenToWorldPoint(Input.mousePosition);
        }

        public bool GetActionPressed(InputAction action) {
            return requestedActions.Contains(action);
        }

        public enum InputAction
        {
            Dash,
            Shoot,
            SpeedUp,
            SlowDown
        }
    }
}