using System.Collections;
using ScriptableObjects;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public PlayerData playerData => m_playerData;
    
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerData m_playerData;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [Header("Dash")]
    [SerializeField] private AnimationCurve dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashRefreshCooldown;
    
    private Rigidbody2D rb;
    
    private Vector2 mousePos;
    private Vector2 dashDir;
    private Vector2 moveDir;
    private bool canDash = true;
    private bool isDashing;
    private bool isFirstPhaseDash;
    private float dashStartTime;

    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() 
    {
        moveDir.x = playerInput.GetAxisRaw(Axis.X);
        moveDir.y = playerInput.GetAxisRaw(Axis.Y);
        moveDir.Normalize();

        if (isDashing)
        {
            rb.AddForce(dashDir * (dashSpeed.Evaluate((Time.time - dashStartTime) / (dashDuration * 2)) * moveSpeed));

            if (isFirstPhaseDash)
            {
                return;
            }
        }

        playerMovement.ApplyMovement(moveDir);
        playerMovement.ApplyRotation(playerInput.GetMousePos());
        ApplyDash();
    }

    private void ApplyDash() 
    {
        if (canDash && playerInput.GetActionPressed(PlayerInput.InputAction.Dash)) 
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        dashDir = moveDir;
        if (dashDir == Vector2.zero) dashDir = Vector2.up;

        canDash = false;
        isDashing = true;
        isFirstPhaseDash = true;
        dashStartTime = Time.time;
        
        yield return new WaitForSeconds(dashDuration);
        isFirstPhaseDash = false;
        
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        
        yield return new WaitForSeconds(dashRefreshCooldown);
        canDash = true;
    }
}
