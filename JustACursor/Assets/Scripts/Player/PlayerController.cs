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
        public PlayerData data => playerData;
        public bool isDashing => playerDash.isDashing;
        public bool invincible
        {
            get => playerCollision.isInvincible;
            set => playerCollision.isInvincible = value;
        }
        
        [SerializeField] private PlayerData playerData;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerShoot playerShoot;
        [SerializeField] private PlayerDash playerDash;
        [SerializeField] private PlayerEnergy playerEnergy;
        [SerializeField] private PlayerDeviceHandler playerDeviceHandler;
        [SerializeField] private PlayerRespawn playerRespawn;
        [SerializeField] private PlayerCollision playerCollision;

        private PlayerInputs inputs;
        private Camera mainCamera;
        private Vector2 moveDirection;
        private Vector2 lookPosition;
        private Vector2 lastDir;
        private IEnumerator stopMovingEnumerator;

        private Vector2 dashDirection => playerDash.dashDirection;
        private bool isAlive => playerRespawn.isAlive;

        public static Vector3 PlayerPosition { get; private set; }

        private void Start() {
            inputs = new PlayerInputs();
            inputs.Enable();
            
            mainCamera = Camera.main;
        }

        private void Update() {
            moveDirection = inputs.Player.Move.ReadValue<Vector2>().normalized;
            PlayerPosition = transform.position; 

            HandleDash();
            HandleMovement();
            HandleRotation();
            HandleShoot();
            HandleEnergy();
        }
        
        private void HandleDash() {
            if (inputs.Player.Dash.WasPressedThisFrame())
            {
                playerDash.HandleDashInput(moveDirection);
            }
            
            if (playerDash.isDashing)
            {
                playerDash.DashMovement(moveDirection);
            }
        }

        private void HandleMovement() {
            if (isDashing) return;
            
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
        }

        private void HandleRotation() {
            playerMovement.LookForward(isDashing && playerDash.isFirstPhase ? dashDirection : moveDirection);
        }

        private void HandleShoot() {
            if (isDashing) return;
            if (inputs.Player.Shoot.IsPressed())
            {
                playerShoot.Shoot();
            }
            
            if (playerDeviceHandler.currentAimMethod == PlayerDeviceHandler.AimMethod.Mouse) MouseAim();
            else GamepadAim();
        }

        private void HandleEnergy() {
            if (inputs.Player.SlowDown.IsPressed()) playerEnergy.SlowDownTime();
            else if (inputs.Player.SpeedUp.IsPressed()) playerEnergy.SpeedUpTime();
            else playerEnergy.ResetSpeed();
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
