using System;
using UnityEngine;

namespace LegacyBosses.Conditions
{
    [Serializable]
    public struct Cdt_CenterDistance : ICondition
    {
        [SerializeField] private Entity targetType;
        [SerializeField] private RelativePosition checkType;
        [SerializeField] private float distance;
        
        public bool Check(Boss boss)
        {
            Vector2 targetPosition = HandleTargetType(boss);

            return HandleCheckType(boss.mover.Room.DistanceToCenter(targetPosition));
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

        private bool HandleCheckType(float relativeDistance)
        {
            return checkType switch
            {
                RelativePosition.Inside => relativeDistance < distance,
                RelativePosition.Outside => relativeDistance > distance,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        
    }
}