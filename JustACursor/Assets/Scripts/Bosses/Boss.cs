using System;
using System.Threading.Tasks;
using Bosses.Dependencies;
using Bosses.Instructions;
using BulletPro;
using Player;
using UnityEngine;

namespace Bosses
{
    public enum BossPhase { None = -1, One = 0, Two = 1, Three = 2 }
    

    public abstract class Boss : MonoBehaviour, IDamageable
    {
        public BulletEmitter[] bulletEmitter => emitters;
        public int maxHP => bossData.startingHP;
        public BossMovement mover => movementHandler;
        public PlayerController targetedPlayer => player;
        
        public Health health;

        [SerializeField] protected PlayerController player;
        [SerializeField] protected BossData bossData;
        [SerializeField] protected BossAnimations animator;
        [SerializeField] private BossMovement movementHandler;
        [SerializeField] private BulletEmitter[] emitters = new BulletEmitter[3];
        
        protected bool isPaused;
        
        private Instruction<Boss> currentInstruction;
        protected BossPhase currentBossPhase;
        private bool isFrozen;
        
        protected virtual void Start()
        {
            DebugStart();
            health.Init(bossData.startingHP);
        }

        protected virtual void Update()
        {
            UpdateDebugInput();

            if (isPaused) return;

            HandlePatterns();
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
            if (currentInstruction == null)
            {
                currentInstruction = bossData.phaseResolvers[(int) currentBossPhase].Resolve(this);
            }
            switch (currentInstruction.phase)
            {
                case InstructionPhase.None:
                    currentInstruction = bossData.phaseResolvers[(int) currentBossPhase].Resolve(this);
                    break;
                case InstructionPhase.Start:
                    currentInstruction.Play(this);
                    break;
                case InstructionPhase.Update:
                    currentInstruction.Update();
                    break;
                case InstructionPhase.Stop:
                    currentInstruction = currentInstruction.Stop();
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

        protected virtual void StopCurrentPattern()
        {
            currentInstruction.Stop();
        }

        

        #region Debug

        [Header("DEBUG")]
        [SerializeField] private bool overridePhaseOnStart;
        [SerializeField] private BossPhase phaseOverride = BossPhase.One;
        [SerializeField] private KeyCode doPhaseOverride = KeyCode.G;
        [SerializeField] private KeyCode freeze = KeyCode.F;
        [SerializeField] private KeyCode setHealthToOne = KeyCode.H;
        [SerializeField] private KeyCode skipPattern = KeyCode.J;

        private void DebugStart()
        {
            if (overridePhaseOnStart) 
                SetBossPhase(phaseOverride);
        }

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

            for (int i = 0; i < 3; i++) // TODO phases[(int)currentBossPhase].attackPatterns[currentPatternIndex].emitterProfiles.length
            {
                if (isFrozen) bulletEmitter[i].Pause(PlayOptions.AllBullets);
                else bulletEmitter[i].Play(PlayOptions.AllBullets);
            }
        }

        [ContextMenu("Reset Boss")]
        private void Reset()
        {
            GetComponent<BulletReceiver>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
            transform.gameObject.SetActive(true);
            isFrozen = false;
            isPaused = false;
        }

        #endregion
    }
}