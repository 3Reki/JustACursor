using BulletPro;
using Player;
using UnityEngine;
using UnityEngine.Events;

public enum AttackMode { NONE, Idle, Circle, Cone, Dash }

public class BossFSM_Virus : BasicBossFSM
{
    [Header("- VIRUS -")]
    [SerializeField] private Energy energy;
    [SerializeField] private BoxCollider2D levelHeight;
    [SerializeField] private BoxCollider2D levelLength;
    private Vector2 levelCenter;
    private bool isMoving, hasReachedDestination; 
    [SerializeField] private Transform[] coneFirePoints = new Transform[0];
    [SerializeField, Range(1, 30)] private float moveSpeed = 10f;

    private AttackMode attackMode;
    private Vector3 currentPlayerPosition;
    private Vector3 destination; 

    [Header("Time Freeze")]
    [SerializeField, Range(1f, 10f)] private float accelerationValue = 4;
    [SerializeField, Range(0f, 1f)] private float  decelerationValue = 0.25f;
    // bullet speed, lifespan, delayBetweenShots

    private void Start()
    {
        Init();
        attackMode = AttackMode.Idle;

        levelCenter = new Vector2(levelLength.bounds.center.x, levelHeight.bounds.center.y + levelHeight.bounds.extents.x);
    } 

    private void SetTimeSpeed(float value)
    {
        BulletModuleMovement.SpeedMultiplier = value;
    }

    private void Update()
    {
        // DEBUG
        UpdateDebugInput();

        // placeholder for reset of mechanic
        if (Input.GetKeyDown(KeyCode.X))
        {
            SetTimeSpeed(1f);
            // SetNewParam(0, "Speed", 100);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            SetTimeSpeed(accelerationValue);
            energy.SpeedUpTime();

            // SetNewParam(0, "Speed", 100);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            SetTimeSpeed(decelerationValue);
            energy.SlowDownTime();

            // SetNewParam(0, "Speed", 100);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            PatternIndex = PatternIndex == 1 ? 2 : 1;

            StopPatterns();
            isMoving = true; 

            attackMode = AttackMode.Circle;
            destination = levelCenter;
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            PatternIndex = 0;

            StopPatterns();
            isMoving = true;

            attackMode = AttackMode.Cone;
            currentPlayerPosition = PlayerController.PlayerPos;
            destination = GetFarthestPositionFromPlayer();
        }
         
        if (Vector3.Distance(destination, transform.position) <= 0.05f && isMoving)
        {
            isMoving = false;
            transform.position = destination;
            float angle = Vector3.SignedAngle(transform.up, (PlayerController.PlayerPos - transform.position), transform.forward);

            transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + angle); 
            SetBossPhase(BossData.CurrentBossPhase, PatternIndex);
            PlayPatterns();
        }
        // TODO: pouvoir alterner entre plusieurs patterns s'ils font partie de la même phase (par ex: Cone et Cercle)
        // TODO: avoir une liste aussi pour les patterns de déplacement

        if (isMoving)
        {
            switch (attackMode)
            {
                case AttackMode.Circle:
                    GoToCenter();
                    break;
                case AttackMode.Cone:
                    GoFarFromPlayer();
                    break;
            }
        }
    }

    private void GoToCenter()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime); 
    }

    // REFACTORING : abstractable for reuse
    private Vector3 GetFarthestPositionFromPlayer()
    {
        int farthestIndex = 0;
        float currentClosestDistance = 0;

        for (int i = 0; i < coneFirePoints.Length; i++)
        {
            float temp = Vector3.Distance(currentPlayerPosition, coneFirePoints[i].position);
            if (temp > currentClosestDistance)
            {
                currentClosestDistance = temp;
                farthestIndex = i;
            }
        }
        return coneFirePoints[farthestIndex].position; 
    }

    private void GoFarFromPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
    }
}
