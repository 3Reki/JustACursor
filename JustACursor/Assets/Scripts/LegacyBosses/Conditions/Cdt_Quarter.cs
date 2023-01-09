using System;
using LD;
using UnityEngine;

namespace LegacyBosses.Conditions
{
    [Serializable]
    public struct Cdt_Quarter : ICondition
    {
        [SerializeField] private Entity targetType;
        [SerializeField] private Room.Quarter quarter;
        [SerializeField] private RelativePosition checkType;
        
        public bool Check(Boss boss)
        {
            Vector2 targetPosition = HandleTargetType(boss);

            return HandleCheckType(boss.mover.Room.IsInsideQuarter(targetPosition, quarter));
        }

        private Vector2 HandleTargetType(Boss boss)
        {
            return targetType switch
            {
                Entity.Player => boss.targetedPlayer.transform.position,
                Entity.Boss => boss.transform.position,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private bool HandleCheckType(bool isInside)
        {
            return checkType switch
            {
                RelativePosition.Inside => isInside,
                RelativePosition.Outside => !isInside,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}