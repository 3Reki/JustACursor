using System;
using Bosses.Dependencies;
using Bosses.Patterns;
using UnityEngine;

namespace Bosses
{
    public class BossSound : Boss
    {
        public int droneCount => drones.Length;
        
        [Space(15)]
        [Header("=== Sound Boss ===")]
        [SerializeField] private TempDrone[] drones = new TempDrone[12];
        [SerializeField] private Resolver<BossSound>[] dronesResolver;
        
        private Pattern<BossSound> currentDronePattern;

        protected override void Update()
        {
            base.Update();

            if (isPaused) return;
            
            HandleDrones();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var otherHealth = other.gameObject.GetComponent<Health>();
            if (otherHealth)
            {
                otherHealth.LoseHealth(1);
            }
        }

        public TempDrone GetDrone(int i)
        {
            return drones[i];
        }

        private void HandleDrones()
        {
            if (currentDronePattern == null)
            {
                currentDronePattern = dronesResolver[(int) currentBossPhase].Resolve(this);
            }
            switch (currentDronePattern.phase)
            {
                case PatternPhase.None:
                    currentDronePattern = dronesResolver[(int) currentBossPhase].Resolve(this);
                    break;
                case PatternPhase.Start:
                    currentDronePattern.Play(this);
                    break;
                case PatternPhase.Update:
                    currentDronePattern.Update();
                    break;
                case PatternPhase.Stop:
                    currentDronePattern = currentDronePattern.Stop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}