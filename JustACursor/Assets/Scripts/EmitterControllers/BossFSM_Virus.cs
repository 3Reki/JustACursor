using BulletPro;
using Player;
using UnityEngine;
using UnityEngine.Events;

public enum MovementMode { NONE, Idle, ToCenter, ToConeFirePoint }

public class BossFSM_Virus : BasicBossFSM
{
    [Header("- VIRUS -")]
    [SerializeField] private Energy energy;
    [SerializeField] private BoxCollider2D levelHeight;
    [SerializeField] private BoxCollider2D levelLength;
    private Vector2 levelCenter;
    private bool move;
    private readonly float moveSpeed = 5f;
    [SerializeField] private Transform[] coneFirePoints = new Transform[0];
    private MovementMode movementMode;
    private Vector3 currentPlayerPosition;

    [Header("Time Freeze")]
    [SerializeField, Range(1f, 10f)] private float accelerationValue = 4;
    [SerializeField, Range(0f, 1f)] private float  decelerationValue = 0.25f;
    // bullet speed, lifespan, delayBetweenShots

    private void Start()
    {
        Init();
        movementMode = MovementMode.Idle;

        levelCenter = new Vector2(levelLength.bounds.center.x, levelHeight.bounds.center.y + levelHeight.bounds.extents.x);
    } 

    private void SetTimeSpeed(float value)
    {
        BulletModuleMovement.TIME_SPEED_MULTIPLIER = value;
    }

    private void Update()
    {
        // DEBUG
        UpdateDebugInput();

        if (Input.GetKeyDown(KeyCode.X))
        {
            SetTimeSpeed(1f);
            // SetNewParam(0, "Speed", 100);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            SetTimeSpeed(accelerationValue);
            // SetNewParam(0, "Speed", 100);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            SetTimeSpeed(decelerationValue);
            // SetNewParam(0, "Speed", 100);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            move = true;
            // energy.SpeedUpTime();
        }

        /* int closestIndex;
        float currentClosestDistance = 200;  
        if (Input.GetKeyDown(KeyCode.L))
        {
            movementMode = MovementMode.ToConeFirePoint;

            currentPlayerPosition = PlayerController.PlayerPos;

            for (int i = 0; i < coneFirePoints.Length; i++)
            {
                currentClosestDistance = Vector3.Distance(currentPlayerPosition, coneFirePoints[i].position)
            }
        } */

        if (move) GoToCenter();
    }

    private void GoToCenter()
    {
        movementMode = MovementMode.ToCenter;

        move = Vector3.Distance(levelCenter, transform.position) >= 0.05f;       
        transform.position = Vector3.MoveTowards(transform.position, levelCenter, moveSpeed * Time.deltaTime); 
    }

    private void GoFarFromPlayer()
    {

    }
}
