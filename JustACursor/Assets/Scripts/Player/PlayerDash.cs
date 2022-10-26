using System.Collections;
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
        private float dashStartTime;
    
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
            dashStartTime = Time.time;

            yield return new WaitForSeconds(playerData.dashFirstPhaseDuration); // TODO : cache WaitForSeconds (wait for GD to look for good values)
            isFirstPhase = false;

            yield return new WaitForSeconds(playerData.dashDuration - playerData.dashFirstPhaseDuration);
            isDashing = false;

            yield return new WaitForSeconds(playerData.dashRefreshCooldown);
            canDash = true;
        }
    }
}