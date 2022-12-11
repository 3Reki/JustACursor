using System;
using System.Threading.Tasks;
using Bosses.Patterns;
using BulletPro;
using Player;
using UnityEngine;

namespace Bosses
{
    public enum BossPhase { None = -1, One = 0, Two = 1, Three = 2 }
    public enum PatternPhase { None, Start, Update, Stop }

    public abstract class Boss : MonoBehaviour, IDamageable
    {
        public BulletEmitter[] bulletEmitter => emitters;
        public int maxHP => bossData.startingHP;
        public BossPhase currentBossPhase { get; private set; }
        public BossMovement mover => movementHandler;
        public PlayerController targetedPlayer => player;
        protected int currentPatternIndex { get; set; }
        public PatternPhase currentPatternPhase = PatternPhase.None;

        [SerializeField] protected PlayerController player;
        [SerializeField] protected BossData bossData;
        [SerializeField] private BossMovement movementHandler;
        [SerializeField] protected BossAnimations animator;
        [SerializeField] private BulletEmitter[] emitters = new BulletEmitter[3]; // must be used only for init
        public Health health;
        
        private Pattern currentPattern;
        private bool isFrozen;
        private bool isPaused;

        private void Awake()
        {
            health.Init(bossData.startingHP);
        }

        private void Start()
        {
            Init();
        }

        protected virtual void Update()
        {
            UpdateDebugInput();

            if (isPaused) return;

            HandlePatterns();
        }

        private void Init()
        {
            currentPatternIndex = 0;
            isPaused = false;
            isFrozen = false;
            //patternCooldownTimer = bossData.patternCooldown;
        }
        
        public void Damage(BulletPro.Bullet bullet, Vector3 hitPoint)
        {
            health.LoseHealth(bullet.moduleParameters.GetInt("Damage"));
            
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
            }
        }
        
        protected bool CheckPhase2HPThreshold() => health.GetRatio() <= bossData.phase2HPThreshold;

        protected bool CheckPhase3HPThreshold() => health.GetRatio() <= bossData.phase3HPThreshold;

        private void HandlePatterns()
        {
            switch (currentPatternPhase)
            {
                case PatternPhase.None:
                    currentPattern = bossData.phaseResolvers[(int) currentBossPhase].Resolve(this);
                    break;
                case PatternPhase.Start:
                    currentPattern.Play(this);
                    break;
                case PatternPhase.Update:
                    currentPattern.Update();
                    break;
                case PatternPhase.Stop:
                    currentPattern = currentPattern.Stop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async void SetBossPhase(BossPhase newPhase)
        {
            StopCurrentPattern();
            animator.ChangePhase();
            isPaused = true;
        
            await Task.Delay((int) (animator.phaseChangeAnimLength * 1000));
            
            currentBossPhase = newPhase;
            isPaused = false;
        }

        public void Heal()
        {
            
        }

        public void Hit()
        {
            animator.Hit();
        }

        public async void Die()
        {
            StopCurrentPattern();
            isPaused = true;

            animator.Die();
            GetComponent<BulletReceiver>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;

            await Task.Delay((int) (animator.deathAnimLength * 1000));
            
            transform.root.gameObject.SetActive(false);
        }

        protected void StopCurrentPattern()
        {
            currentPattern.Stop();
            currentPatternPhase = PatternPhase.None;
        }

        

        #region Debug

        [Header("DEBUG")]
        [Tooltip("0 means don't play the pattern at this index. 1 means play it;")] public int[] playMask = { 1, 1, 1 };
        [SerializeField] private bool overridePhaseOnStart;
        [SerializeField] private BossPhase phaseOverride = BossPhase.One;
        [SerializeField] private KeyCode doPhaseOverride = KeyCode.G;
        [SerializeField] private KeyCode freeze = KeyCode.F;
        [SerializeField] private KeyCode refresh = KeyCode.R;
        [SerializeField] private KeyCode setHealthToOne = KeyCode.H;
        [SerializeField] private KeyCode skipPattern = KeyCode.J;
        
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

            if (Input.GetKeyDown(setHealthToOne))
            {
                health.SetHealth(1);
                animator.Hit();
                SetBossPhase(BossPhase.Three);
            }

            if (Input.GetKeyDown(skipPattern))
            {
                StopCurrentPattern();
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
                    // Debug.Log("playing pattern: " + bossData.phases[(int)currentBossPhase].attackPatterns[currentPatternIndex]);
                }
            }
        }

        [ContextMenu("Reset Boss")]
        private void Reset()
        {
            GetComponent<BulletReceiver>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
            transform.gameObject.SetActive(true);
            Init();
        }

        #endregion
    }
}