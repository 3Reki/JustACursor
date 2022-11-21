using Bosses.Patterns;
using BulletPro;
using DG.Tweening;
using UnityEngine;
using System.Collections;
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

        [Header("Time Freeze")]
        [SerializeField, Range(1f, 10f)] private float accelerationValue = 4;
        [SerializeField, Range(0f, 1f)] private float  decelerationValue = 0.25f;
        // bullet speed, lifespan, delayBetweenShots

        private Vector2 levelCenter;
        private bool hasReachedDestination;

        // TEST
        private float fireRate = 1f;
        private bool canShoot = true;

        PatternParams currentPatternParams;

        private void Start()
        {
            for (int i = 0; i < bossData.phases.Length; i++)
            {
                foreach (Pattern pat in bossData.phases[i].attackPatterns)
                {
                    pat.SetTargetBoss(this);
                }
            }
            Init();

            PlayPattern(currentBossPhase, currentPatternIndex);
            patternTimer = GetPattern(currentBossPhase, currentPatternIndex).length;

            // Debug.Log(bulletEmitter[0].emitterProfile.rootBullet.customParameters[0].floatValue.
            Bounds bounds = levelHeight.bounds;
            levelCenter = new Vector2(levelLength.bounds.center.x, bounds.center.y + bounds.extents.x);

            currentPatternParams = bulletEmitter[0].emitterProfile.GetPattern("Pattern");
            //Debug.Log("rotation: " + pp.instructionLists[0].instructions[3].rotation);
            //Debug.Log("speed: " + pp.instructionLists[0].instructions[3].speedValue);
            //Debug.Log("localMovement" + pp.instructionLists[0].instructions[3].localMovement);
            //Debug.Log("globalMovement" + pp.instructionLists[0].instructions[3].globalMovement);

            //rotation = new DynamicFloat(40f);
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

        public void GoToCenter(float moveDuration)
        {
            MoveTo(levelCenter, moveDuration);
        }

        public void GoToRandomCorner(float moveDuration)
        {
            MoveTo(GetRandomCorner(), moveDuration);
        }

        protected override void UpdateDebugInput()
        {
            base.UpdateDebugInput();
            
            // placeholder for reset of mechanic
            // fireRate=x is for patterns like AP_BulletInCone (loopCount=1)
            // direct access to rotation/waitTime is for patterns like AP_SimpleSpiral (looopMode=endless)
            if (Input.GetKeyDown(KeyCode.X))
            {
                SetTimeSpeed(1f);
                //fireRate = 1f;
                currentPatternParams.instructionLists[0].instructions[2].rotation = new DynamicFloat(20);
                currentPatternParams.instructionLists[0].instructions[3].waitTime = new DynamicFloat(0.2f);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                SetTimeSpeed(accelerationValue);
                energy.SpeedUpTime();
                //fireRate = 0.2f;
                currentPatternParams.instructionLists[0].instructions[2].rotation = new DynamicFloat(5);
                currentPatternParams.instructionLists[0].instructions[3].waitTime = new DynamicFloat(0.01f);
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                SetTimeSpeed(decelerationValue);
                energy.SlowDownTime();
                //fireRate = 3f;
                currentPatternParams.instructionLists[0].instructions[2].rotation = new DynamicFloat(60);
                currentPatternParams.instructionLists[0].instructions[3].waitTime = new DynamicFloat(1f);
            }

            //Debug.Log(currentPatternParams.instructionLists[0].instructions[2].rotation.baseValue);
            //Debug.Log(currentPatternParams.instructionLists[0].instructions[2].rotation.root.defaultValue);

            //Debug.Log(currentPatternParams.instructionLists[0].instructions[3].waitTime.baseValue);
            //Debug.Log(currentPatternParams.instructionLists[0].instructions[3].waitTime.root.defaultValue);

            if (Input.GetKeyDown(KeyCode.M))
            {
                currentPatternIndex = currentPatternIndex == 1 ? 2 : 1;

                StopCurrentPhasePatterns();
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                currentPatternIndex = 0;

                StopCurrentPhasePatterns();
            }
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
            Vector3 currentPlayerPosition = player.transform.position;

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

        private void MoveTo(Vector3 dest, float moveDuration)
        {
            //transform.position = dest;
            transform.DOMove(dest, moveDuration);
            //transform.position = Vector3.MoveTowards(transform.position, dest, moveSpeed * Time.deltaTime);
        }

        public void Shoot()
        {
            if (!canShoot) return;
            bulletEmitter[0].Play();
            StartCoroutine(ShootCooldown());
        }

        private IEnumerator ShootCooldown()
        {
            canShoot = false;
            yield return new WaitForSeconds(fireRate);
            canShoot = true;
        }
    }
}
