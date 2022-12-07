using System;
using Levels;
using Player;
using UnityEngine;

namespace Bosses.Conditions
{
    [Serializable]
    public struct Cdt_CornerDistance : ICondition
    {
        [SerializeField] private Entity targetType;
        [SerializeField] private Room.Corner corner;
        [SerializeField] private RelativePosition checkType;
        [SerializeField] private float distance;
        
        public bool Check(Boss boss)
        {
            Vector2 targetPosition = HandleTargetType(boss);
            
            if (corner == Room.Corner.Any)
                return HandleCheckType(boss.mover.room.MinDistanceToCorners(targetPosition));

            return HandleCheckType(boss.mover.room.DistanceToCorner(targetPosition, corner));
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