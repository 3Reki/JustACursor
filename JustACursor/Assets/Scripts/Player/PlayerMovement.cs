using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Rigidbody2D rb;

        private float accelerationProgress;
        private float decelerationProgress;
        private float rotationProgress;
        
        private PlayerData data => playerController.Data;

        public void LookAtPosition(Vector2 lookPosition)
        {
            Vector2 lookDir = lookPosition - rb.position;
            float angle = Mathf.Atan2(-lookDir.x, lookDir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        public void LookForward(Vector2 dir)
        {
            if (dir.sqrMagnitude < 0.1f) {
                rotationProgress = 0;
                return;
            }

            var angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle),
                data.rotationCurve.Evaluate(rotationProgress));

            rotationProgress += Time.deltaTime;
        }

        public void Move(Vector2 direction)
        {
            if (playerController.IsDashing)
            {
                accelerationProgress = data.moveAcceleration
                    .keys[data.moveAcceleration.length - 1].time;

                return;
            }

            rb.velocity = GetTargetSpeed(direction,
                data.moveAcceleration.Evaluate(accelerationProgress));
            accelerationProgress += Time.deltaTime;
        }

        public IEnumerator StopMoving(Vector2 direction)
        {
            while (playerController.IsDashing)
            {
                yield return null;
            }
            decelerationProgress = 0;
            accelerationProgress = data.moveAcceleration
                .keys[data.moveAcceleration.length - 1].time;

            while (rb.velocity.magnitude > 0)
            {
                rb.velocity = GetTargetSpeed(direction,
                    data.moveDeceleration.Evaluate(decelerationProgress));

                decelerationProgress += Time.deltaTime;
                accelerationProgress -= Time.deltaTime;

                yield return null;
            }

            while (accelerationProgress > 0)
            {
                accelerationProgress -= Time.deltaTime;
                yield return null;
            }

            accelerationProgress = 0;
        }

        private Vector2 GetTargetSpeed(Vector2 direction, float speedPercentage)
        {
            float targetAngle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;

            Vector2 moveDirection = Quaternion.Euler(0, 0, targetAngle) * Vector3.up;
            Vector2 targetSpeed = moveDirection.normalized * (data.moveSpeed * speedPercentage);

            return targetSpeed;
        }
    }
}