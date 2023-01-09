using System.Threading.Tasks;
using Bosses.Dependencies;
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
        public Health Health => bossBar.Health;

        [SerializeField] protected PlayerController player;
        [SerializeField] protected BossData bossData;
        [SerializeField] private BossMovement movementHandler;
        [SerializeField] protected BossAnimations animator;
        [SerializeField] private BossBar bossBar;
        [SerializeField] private BulletEmitter[] emitters = new BulletEmitter[3];
        
        protected bool isPaused;
        
        protected BossPhase currentBossPhase;
        private bool isFrozen;
        
        protected virtual void Start()
        {
            DebugStart();
            Health.Init(bossData.startingHP);
            bossBar.InitBar();
            StartStateMachine();
        }

        protected virtual void Update()
        {
            UpdateDebugInput();

            if (isPaused) return;

            bossData.phaseResolvers[(int) currentBossPhase].UpdateMachine();
        }

        public void Damage(BulletPro.Bullet bullet, Vector3 hitPoint)
        {
            Health.LoseHealth(bullet.moduleParameters.GetInt("Damage"));
            
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
        
        protected bool CheckPhase2HPThreshold() => Health.GetRatio() <= bossData.phase2HPThreshold;

        protected bool CheckPhase3HPThreshold() => Health.GetRatio() <= bossData.phase3HPThreshold;

        private async void SetBossPhase(BossPhase newPhase)
        {
            StopStateMachine();
            animator.ChangePhase();
            isPaused = true;
        
            await Task.Delay((int) (animator.phaseChangeAnimLength * 1000));
            
            currentBossPhase = newPhase;
            isPaused = false;
            StartStateMachine();
        }

        public void Hit()
        {
            animator.Hit();
            bossBar.UpdateBar();
        }

        public async void Die()
        {
            StopStateMachine();
            GetComponent<BulletReceiver>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            
            isPaused = true;
            animator.Die();
            bossBar.Hide();

            await Task.Delay((int) (animator.deathAnimLength * 1000));
            transform.root.gameObject.SetActive(false);
        }
        
        public void Reset()
        {
            StopStateMachine();
            GetComponent<BulletReceiver>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
            
            transform.gameObject.SetActive(true);
            Health.ResetHealth();
            isFrozen = false;
            isPaused = false;
            currentBossPhase = BossPhase.One;

            for (int i = 0; i < 3; i++)
            {
                bulletEmitter[i].Stop();
                bulletEmitter[i].Kill();
            }
            
            StartStateMachine();
        }
        
        protected virtual void StartStateMachine()
        {
            bossData.phaseResolvers[(int) currentBossPhase].Play(this);
        }

        protected virtual void StopStateMachine()
        {
            bossData.phaseResolvers[(int) currentBossPhase].Stop();
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
            {
                StopStateMachine();
                currentBossPhase = phaseOverride;
            }
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
                Health.SetHealth(1);
                animator.Hit();
                SetBossPhase(BossPhase.Three);
            }

            if (Input.GetKeyDown(skipPattern))
            {
                StopStateMachine();
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
        

        #endregion
    }
}