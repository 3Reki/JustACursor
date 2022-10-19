using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Rigidbody2D rb;

        private Quaternion lastRotation;

        public void ApplyMovement(Vector2 dir) 
        {
            rb.AddForce(dir * playerController.data.moveSpeed);
        }

        public void ApplyRotation(Vector2 lookPosition) 
        {
            Vector2 lookDir = lookPosition - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            lastRotation = Quaternion.Euler(0, 0, angle);
            playerController.transform.rotation = lastRotation;
        }
    }
}