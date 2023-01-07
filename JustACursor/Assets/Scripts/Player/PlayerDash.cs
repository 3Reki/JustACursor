using System.Collections;
using CameraScripts;
using UnityEngine;

namespace Player
{
    public class PlayerDash : MonoBehaviour
    {
        public bool IsDashing { get; private set; }
        public bool isFirstPhase { get; private set; }
        public Vector2 dashDirection { get; private set; }

        [SerializeField] private PlayerController playerController;
        [SerializeField] private Rigidbody2D rb;
        
        [Header("Feedback")]
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject baseTrail;
        [SerializeField] private GameObject dashTrail;
        [SerializeField] private GameObject dashPS;

        private bool canDash = true;
        private float elapsedTime;

        private PlayerData playerData => playerController.Data;

        public void HandleDashInput(Vector2 moveDir)
        {
            if (canDash)
            {
                playerController.StartCoroutine(Dash(moveDir));
            }
        }

        private IEnumerator Dash(Vector2 moveDir)
        {
            animator.Play("Player@Dash");
            baseTrail.SetActive(false);
            dashTrail.SetActive(true);
            dashPS.SetActive(true);
            
            dashDirection = moveDir;
            if (dashDirection == Vector2.zero) dashDirection = rb.transform.right;

            rb.velocity = dashDirection * (playerData.dashSpeed * playerData.moveSpeed);

            canDash = false;
            IsDashing = true;
            isFirstPhase = true;
            playerController.IsInvincible = true;
            CameraController.ShakeCamera(playerData.dashShakeIntensity, playerData.dashShakeDuration);

            yield return new WaitForSeconds(playerData.dashFirstPhaseDuration); 
            // TODO : cache WaitForSeconds (wait for GD to look for good values)
            isFirstPhase = false;

            yield return new WaitForSeconds(playerData.dashDuration - playerData.dashFirstPhaseDuration);
            IsDashing = false;
            playerController.IsInvincible = false;
            rb.velocity = Vector2.zero;

            yield return new WaitForSeconds(playerData.dashRefreshCooldown);
            canDash = true;
            
            baseTrail.SetActive(true);
            dashTrail.SetActive(false);
            dashPS.SetActive(false);
        }

        public void DashMovement(Vector2 direction)
        {
            rb.velocity =
                playerController.Data.moveSpeed * playerController.Data.dashSpeed * dashDirection;

            if (direction == Vector2.zero)
                return;

            float targetAngle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
            Vector2 moveDirection = Quaternion.Euler(0, 0, targetAngle) * Vector3.up;
            Vector2 addedMovement = moveDirection.normalized * (playerController.Data.moveSpeed * playerController.Data.dashSecondPhaseControl);

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