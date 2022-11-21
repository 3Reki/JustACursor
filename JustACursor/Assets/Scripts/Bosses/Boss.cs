using System;
using System.Threading.Tasks;
using Bosses.Patterns;
using BulletPro;
using JetBrains.Annotations;
using Player;
using TMPro;
using UnityEngine;

namespace Bosses
{
    public enum BossPhase { None = -1, One, Two, Three }
    
    [Serializable]
    public struct Phase 
    {
        public BossPhase phase;
        public Pattern[] attackPatterns;

        public Phase(Pattern[] attackPatterns)
        {
            this.attackPatterns = attackPatterns;
            phase = BossPhase.None;
        }
    }

    /// <summary>
    /// This is a simple class to set up boss logic. The heavy part of logic is done on the BossData and called here via enum-based state machine.
    /// Said otherwise, this class is about WHAT and WHEN while BossData is about HOW
    /// </summary>

    public abstract class Boss : MonoBehaviour
    {
        public BulletEmitter[] bulletEmitter { get; private set; }
        public int currentHp { get; private set; }
        public BossPhase currentBossPhase { get; private set; }
        public BossMovement mover => movementHandler;
        protected int currentPatternIndex { get; set; }
        
        [SerializeField] protected PlayerController player;
        [SerializeField] protected BossData bossData;
        [SerializeField] private BossMovement movementHandler;
        [SerializeField] protected BossAnimations animator;
        [SerializeField] private TMP_Text bossHP;
        [SerializeField] private BulletEmitter[] emitters = new BulletEmitter[3]; // must be used only for init
        

        private bool isFrozen;
        private bool isPaused;
        protected float patternCooldownTimer;
        protected float patternTimer = -1;

        private void Start()
        {
            Init();
        }

        protected virtual void Update()
        {
            UpdateDebugInput();

            if (isPaused)
                return;
            
            UpdateTimers();
        }

        protected void Init()
        {
            currentHp = bossData.startingHP;
            currentPatternIndex = 0;
            isPaused = false;
            isFrozen = false;
            patternCooldownTimer = bossData.patternCooldown;
            bulletEmitter = new BulletEmitter[emitters.Length];
            Array.Copy(emitters, bulletEmitter, emitters.Length); 
            bossHP.text = $"{currentHp}";
            SetBossPhase(overridePhaseOnStart ? phaseOverride : BossPhase.One);
        }

        // PLACEHOLDER
        // REFACTORING: should be moved elsewere
        [UsedImplicitly] // Used by Bullet Receiver onHit event
        public void TakeDamage(BulletPro.Bullet bullet, Vector3 hitPoint)
        {
            if (isPaused)
                return;

            currentHp -= 1;
            bossHP.text = $"{currentHp}";
            animator.Hit();

            switch (currentBossPhase)
            {
                case BossPhase.None:
                    Debug.LogError("Boss Phase is NONE");
                    break;
                case BossPhase.One:
                    if (CheckPhase2HPThreshold()) SetBossPhase(BossPhase.Two);
                    break;
                case BossPhase.Two:
                    if (CheckPhase3HPThreshold()) SetBossPhase(BossPhase.Three);
                    break;
                case BossPhase.Three:
                    if (currentHp <= 0) Die();
                    break;
            }
        }

        protected void UpdateTimers()
        {
            patternTimer -= Time.deltaTime;
            if (patternTimer >= 0) return;

            patternCooldownTimer -= Time.deltaTime;
            if (patternCooldownTimer < 0)
            {
                ChangePattern();
            }
        }
        
        private void ChangePattern()
        {
            StopPattern(currentBossPhase, currentPatternIndex);
            currentPatternIndex++;
            currentPatternIndex %= GetPatternCountForPhase(currentBossPhase);
            PlayPattern(currentBossPhase, currentPatternIndex);
        }

        private async void SetBossPhase(BossPhase newPhase)
        {
            StopCurrentPhasePatterns();
            animator.ChangePhase();
            isPaused = true;

            await Task.Delay((int) (animator.phaseChangeAnimLength * 1000));
            
            currentBossPhase = newPhase;
            isPaused = false;
            PlayPattern(newPhase, currentPatternIndex = 0);
        }

        private async void Die()
        {
            Debug.Log("Boss is killed");
            StopCurrentPhasePatterns();
            isPaused = true;

            animator.Die();
            GetComponent<BulletReceiver>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;

            await Task.Delay((int) (animator.deathAnimLength * 1000));
            
            transform.root.gameObject.SetActive(false);
        }

        protected Pattern GetPattern(BossPhase phase, int patternIndex) =>
            bossData.phases[(int) phase].attackPatterns[patternIndex];
        
        /// <summary>
        /// This function changes the profile of the bullet emitter without destroying already instantiated bullets
        /// </summary>
        // misleading naming. This only switches profiles, it does not call Play()
        protected void PlayPattern(BossPhase phase, int patternIndex)
        {
            Pattern requestedPattern = GetPattern(phase, patternIndex);
            requestedPattern.Play();
            patternTimer = requestedPattern.length;
            patternCooldownTimer = bossData.patternCooldown;
        }
        
        protected void StopPattern(BossPhase phase, int patternIndex)
        {
            bossData.phases[(int) phase].attackPatterns[patternIndex].Stop();
        }
        
        /// <summary>
        /// Deactivate the emitter of the current phase
        /// </summary>
        protected void StopCurrentPhasePatterns()
        {
            int length = bossData.phases[(int) currentBossPhase].attackPatterns.Length;
            for (int i = 0; i < length; i++)
            {
                bossData.phases[(int) currentBossPhase].attackPatterns[i].Stop();
            }
        }

        // just because I want to keep the percent trigger at the boss data level and NOT in the EmitterControllers
        protected bool CheckPhase2HPThreshold() => (float)currentHp / bossData.startingHP <= bossData.phase2HPPercentTrigger;

        protected bool CheckPhase3HPThreshold() => (float)currentHp / bossData.startingHP <= bossData.phase3HPPercentTrigger;
        
        public int GetPatternCountForPhase(BossPhase phase) => bossData.phases[(int) phase].attackPatterns.Length;
        
        
        
        #region Debug

        [Header("DEBUG")]
        [Tooltip("0 means don't play the pattern at this index. 1 means play it;")] public int[] playMask = { 1, 1, 1 };
        [SerializeField] private bool overridePhaseOnStart;
        [SerializeField] private BossPhase phaseOverride = BossPhase.One;
        [SerializeField] private KeyCode doPhaseOverride = KeyCode.Space;
        [SerializeField] private KeyCode freeze = KeyCode.F;
        [SerializeField] private KeyCode refresh = KeyCode.R;
        
        protected virtual void UpdateDebugInput()
        {
            // change to new profile
            if (Input.GetKeyDown(doPhaseOverride))
            {
                SetBossPhase(phaseOverride);
            }

            // freeze effect. Placeholder
            if (Input.GetKeyDown(freeze))
            {
                FreezeBossBullets();
            }

            if (Input.GetKeyDown(refresh))
            {
                Debug_RefreshPlayID();
            }
        }

        private void FreezeBossBullets()
        {
            isFrozen = !isFrozen;

            for (int i = 0; i < 1; i++) // TODO phases[(int)currentBossPhase].attackPatterns[currentPatternIndex].emitterProfiles.length
            {
                if (isFrozen) bulletEmitter[i].Pause(PlayOptions.AllBullets);
                else bulletEmitter[i].Play(PlayOptions.AllBullets);
            }
        }

        private void Debug_RefreshPlayID()
        {
            for (int i = 0; i < 1; i++) // TODO phases[(int)currentBossPhase].attackPatterns[currentPatternIndex].emitterProfiles.length
            {
                bulletEmitter[i].Stop(); // BAD : will throw an error if emitters are not filld in order
                if (playMask[i] == 1)
                {
                    bulletEmitter[i].Play();
                    Debug.Log("playing pattern: " + bossData.phases[(int)currentBossPhase].attackPatterns[currentPatternIndex]);
                }
            }
        }

        [ContextMenu("Reset Boss")]
        private void Reset()
        {
            GetComponent<BulletReceiver>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
            transform.root.gameObject.SetActive(true);
            Init();
        }

        #endregion
    }
}