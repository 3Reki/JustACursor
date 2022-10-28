using System.Collections;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerShoot playerShoot;
        [SerializeField] private PlayerDash playerDash;
        [SerializeField] private PlayerEnergy playerEnergy;
        [SerializeField] private PlayerDeviceHandler playerDeviceHandler;

        private PlayerInputs inputs;
        private Camera mainCamera;
        private Vector2 moveDirection;
        private Vector2 lookPosition;
        private Vector2 lastDir;
        private IEnumerator stopMovingEnumerator;

        public PlayerData data => playerData;
        public bool isDashing => playerDash.isDashing;
        public Vector2 dashDirection => playerDash.dashDirection;

        private void Start() {
            inputs = new PlayerInputs();
            inputs.Enable();
            
            mainCamera = Camera.main;
        }

        private void Update() {
            moveDirection = inputs.Player.Move.ReadValue<Vector2>().normalized;
            
            if (inputs.Player.Dash.WasPressedThisFrame())
            {
                playerDash.HandleDashInput(moveDirection);
            }

            if (inputs.Player.SlowDown.IsPressed()) playerEnergy.SlowDownTime();
            else if (inputs.Player.SpeedUp.IsPressed()) playerEnergy.SpeedUpTime();
            else playerEnergy.ResetSpeed();
            
            if (playerDash.isDashing)
            {
                playerDash.DashMovement(moveDirection);
                return;
            }

            if (inputs.Player.Move.WasPressedThisFrame() && stopMovingEnumerator != null)
            {
                StopCoroutine(stopMovingEnumerator);
            }

            if (inputs.Player.Move.IsPressed())
            {
                playerMovement.Move(moveDirection);
                lastDir = moveDirection;
            }
            else if (inputs.Player.Move.WasReleasedThisFrame())
            {
                stopMovingEnumerator = playerMovement.StopMoving(lastDir);
                StartCoroutine(stopMovingEnumerator);
            }
            
            if (inputs.Player.Shoot.IsPressed())
            {
                playerShoot.Shoot();
            }
        }

        private void FixedUpdate() {
            if (isDashing)
            {
                if (playerDash.isFirstPhase)
                {
                    playerMovement.LookForward(playerDash.dashDirection);
                    return;
                }
                playerMovement.LookForward(moveDirection);
                return;
            }
            
            if (inputs.Player.Shoot.IsPressed())
            {
                if (playerDeviceHandler.currentAimMethod == PlayerDeviceHandler.AimMethod.Mouse) MouseAim();
                else GamepadAim();
            }
            else
            {
                playerMovement.LookForward(moveDirection);
            }
        }
        
        private void MouseAim()
        {
            lookPosition = mainCamera.ScreenToWorldPoint(inputs.Player.LookMouse.ReadValue<Vector2>());
            playerMovement.LookAtPosition(lookPosition);
        }
        
        private void GamepadAim()
        {
            lookPosition = (Vector2) transform.position + inputs.Player.LookGamepad.ReadValue<Vector2>();
            if (lookPosition != Vector2.zero) playerMovement.LookAtPosition(lookPosition);
        }
    }
}
