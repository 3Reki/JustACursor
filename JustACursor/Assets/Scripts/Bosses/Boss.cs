using System;
using System.Threading.Tasks;
using BulletPro;
using Data._Source;
using JetBrains.Annotations;
using Player;
using TMPro;
using UnityEngine;

namespace Bosses
{
    public enum BossPhase { None = -1, One, Two, Three }

    /// <summary>
    /// This is a simple class to set up boss logic. The heavy part of logic is done on the BossData and called here via enum-based state machine.
    /// Said otherwise, this class is about WHAT and WHEN while BossData is about HOW
    /// </summary>

    public abstract class Boss : MonoBehaviour
    {
        public BulletEmitter[] bulletEmitter { get; private set; }
        public int currentHp { get; private set; }
        public BossPhase currentBossPhase { get; private set; }
        
        [SerializeField] protected PlayerController player;
        [SerializeField] protected BossData bossData;
        [SerializeField] private TMP_Text bossHP;
        [SerializeField] private BulletEmitter[] emitters = new BulletEmitter[3]; // must be used only for init
        [SerializeField] protected BossAnimations animator;

        [Header("DEBUG")]
        [Tooltip("0 means don't play the pattern at this index. 1 means play it;")] public int[] playMask = { 1, 1, 1 };
        [SerializeField] private bool overridePhaseOnStart;
        [SerializeField] private BossPhase phaseOverride = BossPhase.One;
        [SerializeField] private KeyCode doPhaseOverride = KeyCode.Space;
        [SerializeField] private KeyCode freeze = KeyCode.F;
        [SerializeField] private KeyCode refresh = KeyCode.R;
        protected int currentPatternIndex { get; set; }

        private bool isPaused;

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            UpdateDebugInput();
        }

        protected void Init()
        {
            currentHp = bossData.startingHP;
            bulletEmitter = new BulletEmitter[emitters.Length];
            Array.Copy(emitters, bulletEmitter, emitters.Length); 
            bossHP.text = $"{currentHp}";
            SetBossPhase(overridePhaseOnStart ? phaseOverride : BossPhase.One, 0);
        }

        protected void UpdateDebugInput()
        {
            // change to new profile
            if (Input.GetKeyDown(doPhaseOverride))
            {
                SetBossPhase(phaseOverride, 0);
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
            isPaused = !isPaused;

            for (int i = 0; i < bossData.bossPhases[(int)currentBossPhase].attackPatterns[currentPatternIndex].emiterProfiles.Length; i++)
            {
                if (isPaused) bulletEmitter[i].Pause(PlayOptions.AllBullets);
                else bulletEmitter[i].Play(PlayOptions.AllBullets);
            }
        }

        private void Debug_RefreshPlayID()
        {
            for (int i = 0; i < bossData.bossPhases[(int)currentBossPhase].attackPatterns[currentPatternIndex].emiterProfiles.Length; i++)
            {
                bulletEmitter[i].Stop(); // BAD : will throw an error if emitters are not filld in order
                if (playMask[i] == 1)
                {
                    bulletEmitter[i].Play();
                    Debug.Log("playing pattern: " + bossData.bossPhases[(int)currentBossPhase].attackPatterns[currentPatternIndex].emiterProfiles[i]);
                }
            }
        }

        // PLACEHOLDER
        // REFACTORING: should be moved elsewere
        [UsedImplicitly] // Used by Bullet Receiver onHit event
        public void TakeDamage(BulletPro.Bullet bullet, Vector3 hitPoint)
        {
            currentHp -= 1;
            bossHP.text = $"{currentHp}";
            animator.Hit();

            switch (currentBossPhase)
            {
                case BossPhase.None:
                    Debug.LogError("Boss Phase is NONE");
                    break;
                case BossPhase.One:
                    if (CheckPhase2HPThreshold()) SetBossPhase(BossPhase.Two, 0);
                    break;
                case BossPhase.Two:
                    if (CheckPhase3HPThreshold()) SetBossPhase(BossPhase.Three, 0);
                    break;
                case BossPhase.Three:
                    if (currentHp <= 0) Die();
                    break;
            }
        }

        protected async void SetBossPhase(BossPhase newPhase, int patternIndex)
        {
            StopPatterns();
            animator.ChangePhase();

            await Task.Delay((int) (animator.phaseChangeAnimLength * 1000));
            
            currentBossPhase = newPhase;
        }

        /// <summary>
        /// Deactivate the emitter of the current phase
        /// </summary>
        protected void StopPatterns()
        {
            for (int i = 0; i < bossData.bossPhases[(int)currentBossPhase].attackPatterns[currentPatternIndex].emiterProfiles.Length; i++)
            {
                bulletEmitter[i].Stop(); 
            }
        }

        protected async void Die()
        {
            Debug.Log("Boss is killed");
            StopPatterns();

            animator.Die();
            GetComponent<BulletReceiver>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;

            await Task.Delay((int) (animator.deathAnimLength * 1000));
            
            transform.root.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// This function changes the profile of the bullet emitter without destroying already instantiated bullets
        /// </summary>
        // misleading naming. This only switches profiles, it does not call Play()
        protected void PlayPattern(BossPhase phase, int patternIndex)
        {
            for (int i = 0; i < bossData.bossPhases[(int)phase].attackPatterns[patternIndex].emiterProfiles.Length; i++)
            {
                bulletEmitter[i].SwitchProfile(bossData.bossPhases[(int)phase].attackPatterns[patternIndex].emiterProfiles[i]);
            }
        }

        // just because I want to keep the percent trigger at the boss data level and NOT in the EmitterControllers
        protected bool CheckPhase2HPThreshold() => (float)currentHp / bossData.startingHP <= bossData.phase2HPPercentTrigger;

        protected bool CheckPhase3HPThreshold() => (float)currentHp / bossData.startingHP <= bossData.phase3HPPercentTrigger;
    }
}