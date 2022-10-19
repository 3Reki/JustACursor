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
        private bool isDashing => playerDash.isDashing;

        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerShoot playerShoot;
        [SerializeField] private PlayerDash playerDash;
        [SerializeField] private PlayerEnergy playerEnergy;
        [SerializeField] private PlayerData playerData;
        
        private PlayerInputs inputs;
        private Camera mainCamera;
        
        private Vector2 moveDirection;
        private Vector2 lookPosition;

        private void Start() {
            inputs = new PlayerInputs();
            inputs.Enable();
            
            mainCamera = Camera.main;
        }

        private void Update() {
            if (inputs.Player.Dash.WasPressedThisFrame())
            {
                playerDash.HandleDashInput(moveDirection);
            }

            if (inputs.Player.Shoot.IsPressed())
            {
                playerShoot.Shoot();
            }
            
            if (inputs.Player.SlowDown.IsPressed()) playerEnergy.SlowDownTime();
            else if (inputs.Player.SpeedUp.IsPressed()) playerEnergy.SpeedUpTime();
            else playerEnergy.ResetSpeed();
        }

        private void FixedUpdate() {
            moveDirection = inputs.Player.Move.ReadValue<Vector2>().normalized;
            playerMovement.ApplyMovement(moveDirection);

            //Keyboard
            if (Gamepad.current == null)
            {
                lookPosition = mainCamera.ScreenToWorldPoint(inputs.Player.LookMouse.ReadValue<Vector2>());
                playerMovement.ApplyRotation(lookPosition);
            }
            //Gamepad
            else
            {
                lookPosition = inputs.Player.LookGamepad.ReadValue<Vector2>();
                if (lookPosition != Vector2.zero)
                {
                    playerMovement.ApplyRotation(lookPosition);
                }
            }

            if (!isDashing) return;
            playerDash.HandleDash();

            if (playerDash.isFirstPhase)
            {
                return;
            }
        }
    }
}
