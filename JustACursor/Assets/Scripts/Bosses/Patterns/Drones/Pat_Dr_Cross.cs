using System;
using UnityEngine;

namespace Bosses.Patterns.Drones
{
    
    [CreateAssetMenu(fileName = "Pat_Dr_Cross", menuName = "Just A Cursor/Pattern/Drones/Cross Pattern", order = 0)]
    public class Pat_Dr_Cross : Pattern<BossSound>
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
                    start = Vector2.Lerp(linkedEntity.mover.room.topLeft, linkedEntity.mover.room.bottomLeft, .2f);
                    end = Vector2.Lerp(linkedEntity.mover.room.topLeft, linkedEntity.mover.room.topRight, .2f);
                    rotation = Quaternion.Euler(0, 0, -135);
                    break;
                case 1 :
                    start = Vector2.Lerp(linkedEntity.mover.room.topRight, linkedEntity.mover.room.topLeft, .2f);
                    end = Vector2.Lerp(linkedEntity.mover.room.topRight, linkedEntity.mover.room.bottomRight, .2f);
                    rotation = Quaternion.Euler(0, 0, 135);
                    break;
                case 2 :
                    start = Vector2.Lerp(linkedEntity.mover.room.bottomRight, linkedEntity.mover.room.topRight, .2f);
                    end = Vector2.Lerp(linkedEntity.mover.room.bottomRight, linkedEntity.mover.room.bottomLeft, .2f);
                    rotation = Quaternion.Euler(0, 0, 45);
                    break;
                case 3 :
                    start = Vector2.Lerp(linkedEntity.mover.room.bottomLeft, linkedEntity.mover.room.bottomRight, .2f);
                    end = Vector2.Lerp(linkedEntity.mover.room.bottomLeft, linkedEntity.mover.room.topLeft, .2f);
                    rotation = Quaternion.Euler(0, 0, -45);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Vector2.Lerp(start, end, (index % 3 + 0.5f) / 3);
        }
    }
}