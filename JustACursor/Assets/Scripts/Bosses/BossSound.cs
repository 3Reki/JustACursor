using System;
using Bosses.Dependencies;
using Enemies;
using UnityEngine;

namespace Bosses
{
    public class BossSound : Boss
    {
        public int droneCount => drones.Length;
        
        [Space(15)]
        [Header("=== Sound Boss ===")]
        [SerializeField] private SpeakerMinion[] drones = new SpeakerMinion[12];

        protected override void Update()
        {
            base.Update();

            if (isPaused) return;
            
            ((SoundBossData) bossData).droneResolver[(int) currentBossPhase].UpdateMachine();
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

        protected override void StartStateMachine()
        {
            base.StartStateMachine();
            ((SoundBossData) bossData).droneResolver[(int) currentBossPhase].Play(this);
        }

        protected override void StopStateMachine()
        {
            base.StopStateMachine();
            ((SoundBossData) bossData).droneResolver[(int) currentBossPhase].Stop();
            
        }
    }
}