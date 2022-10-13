using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour 
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    private Vector2 moveDir;

    [Header("Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashRefreshCooldown;
    
    private Rigidbody2D rb;
    private PlayerInput inputs;
    private Vector2 lookDir;
    private Vector2 mousePos;
    private Vector2 dashDir;
    private bool canDash = true;
    private bool isDashing;

    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        inputs = GetComponent<PlayerInput>();
    }

    private void FixedUpdate() 
    {
        moveDir.x = inputs.GetAxisRaw(Axis.X);
        moveDir.y = inputs.GetAxisRaw(Axis.Y);
        moveDir.Normalize();

        mousePos = inputs.GetMousePos();

        ApplyMovement();
        ApplyRotation();
        ApplyDash();
    }

    private void ApplyMovement() 
    {
        rb.AddForce(moveDir * moveSpeed);
    }

    private void ApplyRotation() 
    {
        lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    
    private void ApplyDash() 
    {
        if (canDash && inputs.GetActionPressed(InputAction.Dash)) 
        {
            dashDir = moveDir;
            if (dashDir == Vector2.zero) dashDir = lookDir.normalized;

            canDash = false;
            isDashing = true;
            StartCoroutine(ResetDash());
        }

        if (!isDashing) return;
        
        rb.SetVelocity(Axis.X, dashDir.x * dashSpeed);
        rb.SetVelocity(Axis.Y, dashDir.y * dashSpeed);
    }

    private IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        
        yield return new WaitForSeconds(dashRefreshCooldown);
        canDash = true;
    }
}
