using System;
using UnityEngine;

namespace LegacyBosses.Patterns.Drones
{
    
    [CreateAssetMenu(fileName = "Pat_Dr_Cross", menuName = "Just A Cursor/Pattern/Drones/Cross Pattern", order = 0)]
    public class Pat_Dr_Cross : Pat_Dr_Movement
    {
        public override void Play(BossSound entity)
        {
            base.Play(entity);

            int droneCount = linkedEntity.droneCount;

            for (int i = 0; i < droneCount; i++)
            {
                SetTarget(i, GetPosition(i, out Quaternion rotation), rotation);
            }
        }

        private Vector2 GetPosition(int index, out Quaternion rotation)
        {
            Vector2 start;
            Vector2 end;
            switch (index / 3)
            {
                case 0 :
                    start = Vector2.Lerp(linkedEntity.mover.Room.topLeft, linkedEntity.mover.Room.bottomLeft, .2f);
                    end = Vector2.Lerp(linkedEntity.mover.Room.topLeft, linkedEntity.mover.Room.topRight, .2f);
                    rotation = Quaternion.Euler(0, 0, -135);
                    break;
                case 1 :
                    start = Vector2.Lerp(linkedEntity.mover.Room.topRight, linkedEntity.mover.Room.topLeft, .2f);
                    end = Vector2.Lerp(linkedEntity.mover.Room.topRight, linkedEntity.mover.Room.bottomRight, .2f);
                    rotation = Quaternion.Euler(0, 0, 135);
                    break;
                case 2 :
                    start = Vector2.Lerp(linkedEntity.mover.Room.bottomRight, linkedEntity.mover.Room.topRight, .2f);
                    end = Vector2.Lerp(linkedEntity.mover.Room.bottomRight, linkedEntity.mover.Room.bottomLeft, .2f);
                    rotation = Quaternion.Euler(0, 0, 45);
                    break;
                case 3 :
                    start = Vector2.Lerp(linkedEntity.mover.Room.bottomLeft, linkedEntity.mover.Room.bottomRight, .2f);
                    end = Vector2.Lerp(linkedEntity.mover.Room.bottomLeft, linkedEntity.mover.Room.topLeft, .2f);
                    rotation = Quaternion.Euler(0, 0, -45);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Vector2.Lerp(start, end, (index % 3 + 0.5f) / 3);
        }
    }
}