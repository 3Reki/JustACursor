using System;
using Levels;
using UnityEngine;

namespace Bosses.Patterns.Drones
{
    [CreateAssetMenu(fileName = "Pat_Dr_Half", menuName = "Just A Cursor/Pattern/Drones/Half Room Pattern", order = 0)]
    public class Pat_Dr_HalfRoom : Pattern<BossSound>
    {
        [SerializeField] private Room.Half half;

        public override void Play(BossSound boss)
        {
            base.Play(boss);

            GetPositionAndRotation(out Vector2 start, out Vector2 end, out Quaternion rotation);
            int droneCount = linkedBoss.droneCount;

            for (int i = 0; i < droneCount; i++)
            {
                linkedBoss.GetDrone(i).SetPositionAndRotation(Vector2.Lerp(start, end, (i + 0.5f) / droneCount),
                    rotation);
            }
        }

        private void GetPositionAndRotation(out Vector2 lineStart, out Vector2 lineEnd, out Quaternion rotation)
        {
            switch (half)
            {
                case Room.Half.North:
                    lineStart = linkedBoss.mover.room.middleLeft;
                    lineEnd = linkedBoss.mover.room.middleRight;
                    rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case Room.Half.East:
                    lineStart = linkedBoss.mover.room.topCenter;
                    lineEnd = linkedBoss.mover.room.bottomCenter;
                    rotation = Quaternion.Euler(0, 0, -90);
                    break;
                case Room.Half.South:
                    lineStart = linkedBoss.mover.room.middleLeft;
                    lineEnd = linkedBoss.mover.room.middleRight;
                    rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case Room.Half.West:
                    lineStart = linkedBoss.mover.room.topCenter;
                    lineEnd = linkedBoss.mover.room.bottomCenter;
                    rotation = Quaternion.Euler(0, 0, 90);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}