using System;
using Enemies;
using LegacyBosses.Dependencies;
using LegacyBosses.Instructions;
using UnityEngine;

namespace LegacyBosses
{
    public class BossSound : Boss
    {
        public int droneCount => drones.Length;
        
        [Space(15)]
        [Header("=== Sound Boss ===")]
        [SerializeField] private SpeakerMinion[] drones = new SpeakerMinion[12];
        
        private Instruction<BossSound> currentDronePattern;

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

        public SpeakerMinion GetDrone(int i)
        {
            return drones[i];
        }

        protected override void StopCurrentPattern()
        {
            base.StopCurrentPattern();
            if (currentDronePattern != null)
            {
                currentDronePattern.Stop();
            }
            
        }

        private void HandleDrones()
        {
            if (currentDronePattern == null)
            {
                currentDronePattern = ((SoundBossData) bossData).droneResolvers[(int) currentBossPhase].Resolve(this);
            }
            switch (currentDronePattern.phase)
            {
                case InstructionPhase.None:
                    currentDronePattern = ((SoundBossData) bossData).droneResolvers[(int) currentBossPhase].Resolve(this);
                    break;
                case InstructionPhase.Start:
                    currentDronePattern.Play(this);
                    break;
                case InstructionPhase.Update:
                    currentDronePattern.Update();
                    break;
                case InstructionPhase.Stop:
                    currentDronePattern = currentDronePattern.Stop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}