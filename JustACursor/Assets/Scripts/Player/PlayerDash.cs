using System.Collections;
using ScriptableObjects;
using UnityEngine;

namespace Player
{
    public class PlayerDash : MonoBehaviour
    {
        public bool isDashing { get; private set; }
        public bool isFirstPhase { get; private set; }

        [SerializeField] private PlayerController playerController;
        [SerializeField] private Rigidbody2D rb;

        private Vector2 dashDir;
        private bool canDash = true;
        private float dashStartTime;
    
        private PlayerData playerData => playerController.playerData;

        public void HandleDash()
        {
            float dashCompletionPercentage = (Time.time - dashStartTime) / (playerData.dashDuration * 2);
            rb.AddForce(dashDir * (playerData.dashSpeed.Evaluate(dashCompletionPercentage) * playerData.moveSpeed));
        }

        public void HandleDashInput(Vector2 moveDir)
        {
            if (canDash)
            {
                playerController.StartCoroutine(Dash(moveDir));
            }
        }

        private IEnumerator Dash(Vector2 moveDir)
        {
            dashDir = moveDir;
            if (dashDir == Vector2.zero) dashDir = Vector2.up;

            canDash = false;
            isDashing = true;
            isFirstPhase = true;
            dashStartTime = Time.time;

            yield return new WaitForSeconds(playerData.dashDuration);
            isFirstPhase = false;

            yield return new WaitForSeconds(playerData.dashDuration);
            isDashing = false;

            yield return new WaitForSeconds(playerData.dashRefreshCooldown);
            canDash = true;
        }
    }
}