﻿using System;
using BulletPro;
using Player;
using UnityEngine;

namespace Bosses
{
    public enum AttackMode { None, Idle, Circle, Cone, Dash }

    public class BossVirus : BasicBossFSM
    {
        [Header("- VIRUS -")]
        [SerializeField] private Energy energy;
        [SerializeField] private BoxCollider2D levelHeight;
        [SerializeField] private BoxCollider2D levelLength;
        [SerializeField] private Transform[] coneFirePoints = Array.Empty<Transform>();
        [SerializeField, Range(1, 30)] private float moveSpeed = 10f;
        [SerializeField] private float patternDuration = 2f;
        [SerializeField] private float timeBetweenPatterns = .5f;

        [Header("Time Freeze")]
        [SerializeField, Range(1f, 10f)] private float accelerationValue = 4;
        [SerializeField, Range(0f, 1f)] private float  decelerationValue = 0.25f;
        // bullet speed, lifespan, delayBetweenShots
        
        private Vector2 levelCenter;
        private bool isMoving, hasReachedDestination; 
        private AttackMode attackMode;
        private Vector3 currentPlayerPosition;
        private float patternTime;
        private float patternCooldown;
        private int phasePatternCount;


        private void Start()
        {
            patternTime = patternDuration;
            patternCooldown = timeBetweenPatterns;
            Init();
            RotateTowardsPlayer();
            PlayPattern(currentBossPhase, currentPatternIndex);
            phasePatternCount = bossData.GetPatternCountForPhase(currentBossPhase);
            attackMode = AttackMode.Idle;

            Bounds bounds = levelHeight.bounds;
            levelCenter = new Vector2(levelLength.bounds.center.x, bounds.center.y + bounds.extents.x);
        } 


        private void Update()
        {
            // DEBUG
            UpdateDebugInput();

            TestInputs();

            patternTime -= Time.deltaTime;
            if (patternTime < 0)
            {
                patternCooldown -= Time.deltaTime;
                if (patternCooldown < 0)
                {
                    ChangePattern();
                }
            }
         
            // if (Vector3.Distance(destination, transform.position) <= 0.05f && isMoving)
            // {
            //     isMoving = false;
            //     transform.position = destination;
            //     float angle = Vector3.SignedAngle(transform.up, (PlayerController.PlayerPos - transform.position), transform.forward);
            //
            //     transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + angle); 
            //     SetBossPhase(BossData.CurrentBossPhase, PatternIndex);
            //     PlayPatterns();
            // }
            // TODO: pouvoir alterner entre plusieurs patterns s'ils font partie de la même phase (par ex: Cone et Cercle)
            // TODO: avoir une liste aussi pour les patterns de déplacement

            // if (isMoving)
            // {
            //     switch (attackMode)
            //     {
            //         case AttackMode.Circle:
            //             GoToCenter();
            //             break;
            //         case AttackMode.Cone:
            //             GoFarFromPlayer();
            //             break;
            //     }
            // }
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

                StopPatterns();
                isMoving = true;

                attackMode = AttackMode.Circle;
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                currentPatternIndex = 0;

                StopPatterns();
                isMoving = true;

                attackMode = AttackMode.Cone;
                currentPlayerPosition = PlayerController.PlayerPos;
            }
        }

        private void ChangePattern()
        {
            RotateTowardsPlayer();
            currentPatternIndex++;
            currentPatternIndex %= phasePatternCount;
            PlayPattern(BossPhase.One, currentPatternIndex);
            patternTime = patternDuration;
            patternCooldown = timeBetweenPatterns;
        }

        private void RotateTowardsPlayer()
        {
            Vector3 dir = player.transform.position - transform.position;
            dir.Normalize();
            float zRotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, zRotation - 90);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Health health = other.gameObject.GetComponent<Health>();
            if (health)
            {
                health.LoseHealth(1);
            }
        }

        private void SetTimeSpeed(float value)
        {
            BulletModuleMovement.SpeedMultiplier = value;
        }

        private void GoToCenter()
        {
            TeleportTo(levelCenter);
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
            TeleportTo(GetFarthestPositionFromPlayer());
        }

        private void TeleportTo(Vector3 dest)
        {
            transform.position = Vector3.MoveTowards(transform.position, dest, moveSpeed * Time.deltaTime); 
        }
    }
}