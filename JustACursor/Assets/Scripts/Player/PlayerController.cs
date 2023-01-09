using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        public PlayerData Data => playerData;
        public bool IsDashing => playerDash.IsDashing;
        public bool IsInvincible
        {
            set => playerCollision.IsInvincible = value;
        }
        
        [SerializeField] private PlayerData playerData;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerShoot playerShoot;
        [SerializeField] private PlayerDash playerDash;
        [SerializeField] private PlayerEnergy playerEnergy;
        [SerializeField] private PlayerDeviceHandler playerDeviceHandler;
        [SerializeField] private PlayerCollision playerCollision;
        [SerializeField] private PlayerRespawn playerRespawn;
        [field:SerializeField] public Health Health { get; private set; }

        private PlayerInputs inputs;
        private Camera mainCamera;
        private Vector2 moveDirection;
        private Vector2 lastDir;
        private IEnumerator stopMovingEnumerator;

        private Vector2 dashDirection => playerDash.dashDirection;

        public static Vector3 PlayerPosition { get; private set; }

        private void Start()
        {
            inputs = InputManager.Instance.inputs;
            
            mainCamera = Camera.main;
        }

        private void Update() {
            if (playerRespawn.isRespawning) {
                HandleRespawn();
                return;
            }

            if (Input.GetKey(KeyCode.P))
            {
                Debug.Log(PlayerPosition);
            }
            
            moveDirection = inputs.Player.Move.ReadValue<Vector2>().normalized;
            PlayerPosition = transform.position; 

            HandleDash();
            HandleMovement();
            HandleRotation();
            HandleShoot();
            HandleEnergy();
        }

        private void HandleRespawn()
        {
            if (stopMovingEnumerator == null) return;
                
            StopCoroutine(stopMovingEnumerator);
            playerMovement.Stop();
        }

        private void HandleDash() {
            if (inputs.Player.Dash.WasPressedThisFrame())
            {
                playerDash.HandleDashInput(moveDirection);
            }
            
            if (playerDash.IsDashing)
            {
                playerDash.DashMovement(moveDirection);
            }
        }

        private void HandleMovement() {
            if (IsDashing) return;
            
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
            playerMovement.LookForward(IsDashing && playerDash.isFirstPhase ? dashDirection : moveDirection);
        }

        private void HandleShoot() {
            if (IsDashing || Time.timeScale == 0) return;
            if (inputs.Player.Shoot.IsPressed())
            {
                if (playerDeviceHandler.currentAimMethod == PlayerDeviceHandler.AimMethod.Mouse) MouseAim();
                else GamepadAim();
                
                playerShoot.Shoot();
            }
            else playerShoot.StopShoot();
        }

        private void HandleEnergy() {
            if (inputs.Player.SlowDown.IsPressed()) playerEnergy.SlowDownTime();
            else if (inputs.Player.SpeedUp.IsPressed()) playerEnergy.SpeedUpTime();
            else playerEnergy.ResetSpeed();
        }
        
        private void MouseAim()
        {
            Vector2 lookPosition = mainCamera.ScreenToWorldPoint(inputs.Player.LookMouse.ReadValue<Vector2>());
            playerMovement.LookAtPosition(lookPosition);
        }
        
        private void GamepadAim()
        {
            Vector2 lookDir = inputs.Player.LookGamepad.ReadValue<Vector2>();
            if (lookDir != Vector2.zero)
            {
                playerMovement.LookAtPosition(lookDir + (Vector2) transform.position);
            }
        }
    }
}
