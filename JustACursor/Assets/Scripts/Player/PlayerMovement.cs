using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Rigidbody2D rb;

        private Quaternion lastRotation;

        public void Move(Vector2 dir) 
        {
            rb.AddForce(dir * playerController.data.moveSpeed);
        }

        public void LookAtPosition(Vector2 lookPosition) 
        {
            Vector2 lookDir = lookPosition - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        public void LookForward(Vector2 dir)
        {
            if (dir.sqrMagnitude < 0.1f)
            {
                return;
            }

            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), playerController.data.rotationSpeed);
        }
    }
}