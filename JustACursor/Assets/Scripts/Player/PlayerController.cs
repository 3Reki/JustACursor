using ScriptableObjects;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        public PlayerData playerData => m_playerData;
        public bool isDashing => playerDash.isDashing;
    
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerDash playerDash;
        [SerializeField] private PlayerData m_playerData;

        private Vector2 moveDir;

        private void FixedUpdate() 
        {
            moveDir.x = playerInput.GetAxisRaw(Axis.X);
            moveDir.y = playerInput.GetAxisRaw(Axis.Y);
            moveDir.Normalize();

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
        
        }
    }
}
