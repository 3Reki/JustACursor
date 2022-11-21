﻿using Bosses.Patterns;
using UnityEngine;

namespace Bosses
{
    public class BossVirus : Boss
    {
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
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var health = other.gameObject.GetComponent<Health>();
            if (health)
            {
                health.LoseHealth(1);
            }
        }

        protected override void UpdateDebugInput()
        {
            base.UpdateDebugInput();

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
    }
}