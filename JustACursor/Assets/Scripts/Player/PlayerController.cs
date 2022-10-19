using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        private const string mouseScheme = "Keyboard & Mouse";

        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerShoot playerShoot;
        [SerializeField] private PlayerDash playerDash;
        [SerializeField] private PlayerEnergy playerEnergy;
        [SerializeField] private PlayerData playerData;
        [SerializeField] private PlayerInput playerInput;
        
        private PlayerInputs inputs;
        private Camera mainCamera;
        private Vector2 moveDirection;
        private Vector2 lookPosition;
        private string previousScheme;
        
        public PlayerData data => playerData;
        private bool isDashing => playerDash.isDashing;

        private void Start() {
            inputs = new PlayerInputs();
            inputs.Enable();
            
            mainCamera = Camera.main;
            previousScheme = playerInput.currentControlScheme;
            aimMethod = MouseAim;
        }

        private void OnEnable()
        {
            playerInput.onControlsChanged += OnControlsChanged;
        }

        private void OnDisable()
        {
            playerInput.onControlsChanged -= OnControlsChanged;
        }

        private void Update() {
            if (inputs.Player.Dash.WasPressedThisFrame())
            {
                playerDash.HandleDashInput(moveDirection);
            }

            if (inputs.Player.SlowDown.IsPressed()) playerEnergy.SlowDownTime();
            else if (inputs.Player.SpeedUp.IsPressed()) playerEnergy.SpeedUpTime();
            else playerEnergy.ResetSpeed();
            
            if (playerDash.isFirstPhase)
            {
                return;
            }
            
            if (inputs.Player.Shoot.IsPressed())
            {
                playerShoot.Shoot();
            }
        }

        private void FixedUpdate() {
            if (isDashing)
                playerDash.HandleDash();
            
            if (playerDash.isFirstPhase)
            {
                return;
            }
            
            moveDirection = inputs.Player.Move.ReadValue<Vector2>().normalized;
            playerMovement.Move(moveDirection);

            if (inputs.Player.Shoot.IsPressed())
            {
                aimMethod();
            }
            else
            {
                playerMovement.LookForward(moveDirection);
            }
        }

        private void GamepadAim()
        {
            lookPosition = (Vector2) transform.position + inputs.Player.LookGamepad.ReadValue<Vector2>();
            if (lookPosition != Vector2.zero)
            {
                playerMovement.LookAtPosition(lookPosition);
            }
        }

        private void MouseAim()
        {
            lookPosition = mainCamera.ScreenToWorldPoint(inputs.Player.LookMouse.ReadValue<Vector2>());
            playerMovement.LookAtPosition(lookPosition);
        }

        private void OnControlsChanged(PlayerInput input)
        {
            if (playerInput.currentControlScheme == mouseScheme && previousScheme != mouseScheme)
            {
                Cursor.visible = true;
                aimMethod = MouseAim;
            }
            else
            {
                Cursor.visible = false;
                aimMethod = GamepadAim;
            }

            previousScheme = playerInput.currentControlScheme;
        }

        private delegate void ControllerCallback();

        private ControllerCallback aimMethod;
    }
}
