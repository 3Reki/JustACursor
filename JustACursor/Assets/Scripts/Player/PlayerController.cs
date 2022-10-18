using ScriptableObjects;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        public PlayerData data => playerData;
        public bool isDashing => playerDash.isDashing;
    
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerShoot playerShoot;
        [SerializeField] private PlayerDash playerDash;
        [SerializeField] private PlayerEnergy playerEnergy;
        [SerializeField] private PlayerData playerData;

        private Vector2 moveDir;

        private void FixedUpdate() 
        {
            moveDir.x = playerInput.GetAxisRaw(Axis.X);
            moveDir.y = playerInput.GetAxisRaw(Axis.Y);
            moveDir.Normalize();

            if (playerInput.GetActionPressed(PlayerInput.InputAction.SlowDown))
            {
                playerEnergy.SlowDownTime();
            }
            else if (playerInput.GetActionPressed(PlayerInput.InputAction.SpeedUp))
            {
                playerEnergy.SpeedUpTime();
            }
            else
            {
                playerEnergy.ResetSpeed();
            }

            if (isDashing)
            {
                playerDash.HandleDash();

                if (playerDash.isFirstPhase)
                {
                    return;
                }
            }

            playerMovement.ApplyMovement(moveDir);
            playerMovement.ApplyRotation(playerInput.GetMousePos());

            if (playerInput.GetActionPressed(PlayerInput.InputAction.Dash))
            {
                playerDash.HandleDashInput(moveDir);
            }

            if (playerInput.GetActionPressed(PlayerInput.InputAction.Shoot))
            {
                playerShoot.Shoot();
            }
        }
    }
}
