﻿using System;
using Levels;
using UnityEngine;

namespace Bosses.Conditions
{
    [Serializable]
    public struct Cdt_BossDistance : ICondition
    {
        [SerializeField] private RelativePosition checkType;
        [SerializeField] private float distance;
        
        public bool Check(Boss boss)
        {
            Vector2 targetPosition = boss.targetedPlayer.transform.position;

            return HandleCheckType(Vector2.Distance(boss.transform.position, targetPosition));
        }

        private bool HandleCheckType(float relativeDistance)
        {
            return checkType switch
            {
                RelativePosition.CloserThan => relativeDistance < distance,
                RelativePosition.FartherThan => relativeDistance > distance,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}