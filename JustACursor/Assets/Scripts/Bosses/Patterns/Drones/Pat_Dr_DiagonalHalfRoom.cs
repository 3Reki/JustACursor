using System;
using LD;
using UnityEngine;

namespace Bosses.Patterns.Drones
{
    [CreateAssetMenu(fileName = "Pat_Dr_DiagonalHalf", menuName = "Just A Cursor/Pattern/Drones/Diagonal Half Room Pattern", order = 0)]
    public class Pat_Dr_DiagonalHalfRoom : Pat_Dr_Movement
    {
        [SerializeField] private Room.Quarter startingCorner;
        
        public override void Play(BossSound entity)
        {
            base.Play(entity);

            GetPositionAndRotation(out Vector2 start, out Vector2 end, out Quaternion rotation);
            int droneCount = linkedEntity.droneCount;

            for (int i = 0; i < droneCount; i++)
            {
                SetTarget(i, Vector2.Lerp(start, end, (i + 0.5f) / droneCount),
                    rotation);
            }
        }

        private void GetPositionAndRotation(out Vector2 lineStart, out Vector2 lineEnd, out Quaternion rotation)
        {
            switch (startingCorner)
            {
                case Room.Quarter.NorthWest:
                    lineStart = linkedEntity.mover.Room.topLeft;
                    lineEnd = linkedEntity.mover.Room.bottomRight;
                    rotation = Quaternion.Euler(0, 0, -45);
                    break;
                case Room.Quarter.NorthEast:
                    lineStart = linkedEntity.mover.Room.topRight;
                    lineEnd = linkedEntity.mover.Room.bottomLeft;
                    rotation = Quaternion.Euler(0, 0, -135);
                    break;
                case Room.Quarter.SouthWest:
                    lineStart = linkedEntity.mover.Room.topRight;
                    lineEnd = linkedEntity.mover.Room.bottomLeft;
                    rotation = Quaternion.Euler(0, 0, 45);
                    break;
                case Room.Quarter.SouthEast:
                    lineStart = linkedEntity.mover.Room.topLeft;
                    lineEnd = linkedEntity.mover.Room.bottomRight;
                    rotation = Quaternion.Euler(0, 0, 135);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}