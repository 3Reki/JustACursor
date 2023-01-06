using System;
using UnityEngine;

namespace LegacyBosses.Patterns.Drones
{
    
    public class Pat_Dr_Plus : Pat_Dr_Movement
    {
        
        public override void Play(BossSound entity)
        {
            base.Play(entity);

            int droneCount = linkedEntity.droneCount;

            for (int i = 0; i < droneCount; i++)
            {
                SetTarget(i, GetPosition(i, out Quaternion rotation),
                    rotation);
            }
        }

        private Vector2 GetPosition(int index, out Quaternion rotation)
        {
            Vector2 start;
            Vector2 end;
            switch (index / 3)
            {
                case 0 :
                    start = linkedEntity.mover.Room.topLeft;
                    end = linkedEntity.mover.Room.topRight;
                    rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case 1 :
                    start = linkedEntity.mover.Room.topRight;
                    end = linkedEntity.mover.Room.bottomRight;
                    rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case 2 :
                    start = linkedEntity.mover.Room.bottomRight;
                    end = linkedEntity.mover.Room.bottomLeft;
                    rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 3 :
                    start = linkedEntity.mover.Room.bottomLeft;
                    end = linkedEntity.mover.Room.topLeft;
                    rotation = Quaternion.Euler(0, 0, -90);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Vector2.Lerp(start, end, (index % 3 + 5f) / 12);
        }
    }
}