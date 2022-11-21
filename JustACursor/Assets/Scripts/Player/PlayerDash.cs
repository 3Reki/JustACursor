using System.Collections;
using CameraScripts;
using ScriptableObjects;
using UnityEngine;

namespace Player
{
    public class PlayerDash : MonoBehaviour
    {
        public bool isDashing { get; private set; }
        public bool isFirstPhase { get; private set; }
        public Vector2 dashDirection { get; private set; }

        [SerializeField] private PlayerController playerController;
        [SerializeField] private Rigidbody2D rb;

        private bool canDash = true;
        private float elapsedTime;

        private PlayerData playerData => playerController.data;

        public void HandleDashInput(Vector2 moveDir)
        {
            if (canDash)
            {
                playerController.StartCoroutine(Dash(moveDir));
            }
        }

        private IEnumerator Dash(Vector2 moveDir)
        {
            dashDirection = moveDir;
            if (dashDirection == Vector2.zero) dashDirection = rb.transform.right;

            rb.velocity = dashDirection * (playerData.dashSpeed * playerData.moveSpeed);

            canDash = false;
            isDashing = true;
            isFirstPhase = true;
            playerController.invincible = true;
            CameraController.ShakeCamera(playerData.dashShakeIntensity, playerData.dashShakeDuration);

            yield return new WaitForSeconds(playerData.dashFirstPhaseDuration); 
                // TODO : cache WaitForSeconds (wait for GD to look for good values)
            isFirstPhase = false;

            yield return new WaitForSeconds(playerData.dashDuration - playerData.dashFirstPhaseDuration);
            isDashing = false;
            playerController.invincible = false;
            rb.velocity = Vector2.zero;

            yield return new WaitForSeconds(playerData.dashRefreshCooldown);
            canDash = true;
        }

        public void DashMovement(Vector2 direction)
        {
            rb.velocity =
                playerController.data.moveSpeed * playerController.data.dashSpeed * dashDirection;

            if (direction == Vector2.zero)
                return;

            float targetAngle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
            Vector2 moveDirection = Quaternion.Euler(0, 0, targetAngle) * Vector3.up;
            Vector2 addedMovement = moveDirection.normalized * (playerController.data.moveSpeed * playerController.data.dashSecondPhaseControl);

            if (Vector2.Dot(addedMovement, dashDirection) > 0)
            {
                Vector2 dashPerpendicular = Vector2.Perpendicular(dashDirection);
                addedMovement = (Vector2.Dot(addedMovement, dashPerpendicular) / dashPerpendicular.sqrMagnitude) *
                                dashPerpendicular; // Orthogonal projection of addedMovement on 
            }

            rb.velocity += addedMovement;
            dashDirection = rb.velocity.normalized;
        }
    }
}