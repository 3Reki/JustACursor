using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Rigidbody2D rb;

        public void ApplyMovement(Vector2 dir) 
        {
            rb.AddForce(dir * playerController.playerData.moveSpeed);
        }

        public void ApplyRotation(Vector2 mousePos) 
        {
            Vector2 lookDir = mousePos - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            playerController.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}