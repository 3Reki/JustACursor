using System;
using UnityEngine;

namespace Bosses.Patterns.Drones
{
    [CreateAssetMenu(fileName = "Pat_Dr_Square", menuName = "Just A Cursor/Pattern/Drones/Square Pattern", order = 0)]
    public class Pat_Dr_Square : Pattern<BossSound>
    {
        public override void Play(BossSound entity)
        {
            base.Play(entity);

            int droneCount = linkedEntity.droneCount;

            for (int i = 0; i < droneCount; i++)
            {
                linkedEntity.GetDrone(i).SetPositionAndRotation(GetPosition(i, out Quaternion rotation),
                   rotation);
            }
        }

        public override void Stop() {}

        private Vector2 GetPosition(int index, out Quaternion rotation)
        {
            Vector2 start;
            Vector2 end;
            switch (index / 3)
            {
                case 0 :
                    start = linkedEntity.mover.room.topLeft;
                    end = linkedEntity.mover.room.bottomLeft;
                    rotation = Quaternion.Euler(0, 0, -90);
                    break;
                case 1 :
                    start = linkedEntity.mover.room.topRight;
                    end = linkedEntity.mover.room.topLeft;
                    rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case 2 :
                    start = linkedEntity.mover.room.bottomRight;
                    end = linkedEntity.mover.room.topRight;
                    rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case 3 :
                    start = linkedEntity.mover.room.bottomLeft;
                    end = linkedEntity.mover.room.bottomRight;
                    rotation = Quaternion.Euler(0, 0, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Vector2.Lerp(start, end, (index % 3 + 0.5f) / 12);
        }
    }
}