using System;
using BulletPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bosses
{

    public class BossVirus : Boss
    {
        [Header("- VIRUS -")]
        [SerializeField] private Energy energy;
        [SerializeField] private BoxCollider2D levelHeight;
        [SerializeField] private BoxCollider2D levelLength;
        [SerializeField] private Transform[] coneFirePoints;
        [SerializeField, Range(1, 30)] private float moveSpeed = 10f;
        [SerializeField] private float timeBetweenPatterns = .5f;

        [Header("Time Freeze")]
        [SerializeField, Range(1f, 10f)] private float accelerationValue = 4;
        [SerializeField, Range(0f, 1f)] private float  decelerationValue = 0.25f;
        // bullet speed, lifespan, delayBetweenShots
        
        private Vector2 levelCenter;
        private bool isMoving, hasReachedDestination; 
        private Vector3 currentPlayerPosition;
        private float patternCooldown;
        private int phasePatternCount;
        private float patternTime = -1;


        private void Start()
        {
            patternCooldown = timeBetweenPatterns;
            Init();
            RotateTowardsPlayer();
            PlayPattern(currentBossPhase, currentPatternIndex);
            phasePatternCount = GetPatternCountForPhase(currentBossPhase);

            Bounds bounds = levelHeight.bounds;
            levelCenter = new Vector2(levelLength.bounds.center.x, bounds.center.y + bounds.extents.x);
        } 


        private void Update()
        {
            // DEBUG
            UpdateDebugInput();

            TestInputs();

            patternTime -= Time.deltaTime;
            if (!(patternTime < 0)) return;
            
            patternCooldown -= Time.deltaTime;
            if (patternCooldown < 0)
            {
                ChangePattern();
            }
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            Health health = other.gameObject.GetComponent<Health>();
            if (health)
            {
                health.LoseHealth(1);
            }
        }
        
        public void RotateTowardsPlayer()
        {
            Vector3 dir = player.transform.position - transform.position;
            dir.Normalize();
            float zRotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, zRotation - 90);
        }

        public void GoToCenter()
        {
            TeleportTo(levelCenter);
        }

        public void GoToRandomCorner()
        {
            TeleportTo(GetRandomCorner());
        }

        private void TestInputs()
        {
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
                currentPatternIndex = currentPatternIndex == 1 ? 2 : 1;

                StopCurrentPhasePatterns();
                isMoving = true;
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                currentPatternIndex = 0;

                StopCurrentPhasePatterns();
                isMoving = true;
            }
        }

        private void ChangePattern()
        {
            currentPatternIndex++;
            currentPatternIndex %= phasePatternCount;
            PlayPattern(currentBossPhase, currentPatternIndex);
            patternTime = GetPattern(currentBossPhase, currentPatternIndex).length;
            patternCooldown = timeBetweenPatterns;
        }
        
        private Vector3 GetRandomCorner()
        {
            return coneFirePoints[Random.Range(0, coneFirePoints.Length)].position;
        }

        private void SetTimeSpeed(float value)
        {
            BulletModuleMovement.SpeedMultiplier = value;
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

        private void TeleportTo(Vector3 dest)
        {
            transform.position = dest;
            //transform.position = Vector3.MoveTowards(transform.position, dest, moveSpeed * Time.deltaTime); 
        }
    }
}