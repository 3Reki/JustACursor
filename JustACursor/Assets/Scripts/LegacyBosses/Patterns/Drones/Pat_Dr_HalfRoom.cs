using System;
using LD;
using UnityEngine;

namespace LegacyBosses.Patterns.Drones
{
    [CreateAssetMenu(fileName = "Pat_Dr_Half", menuName = "Just A Cursor/Pattern/Drones/Half Room Pattern", order = 0)]
    public class Pat_Dr_HalfRoom : Pat_Dr_Movement
    {
        [SerializeField] private Room.Half half;
        [SerializeField] private AlternateDirectionMode alternateDirectionMode = AlternateDirectionMode.Simple;

        public override void Play(BossSound entity)
        {
            base.Play(entity);

            GetPosition(out Vector2 start, out Vector2 end);
            int droneCount = linkedEntity.droneCount;

            for (int i = 0; i < droneCount; i++)
            {
                SetTarget(i, Vector2.Lerp(start, end, (i + 0.5f) / droneCount), GetRotation(i, droneCount));
            }
        }

        private void GetPosition(out Vector2 lineStart, out Vector2 lineEnd)
        {
            switch (half)
            {
                case Room.Half.North:
                    lineStart = linkedEntity.mover.Room.middleLeft;
                    lineEnd = linkedEntity.mover.Room.middleRight;
                    break;
                case Room.Half.East:
                    lineStart = linkedEntity.mover.Room.topCenter;
                    lineEnd = linkedEntity.mover.Room.bottomCenter;
                    break;
                case Room.Half.South:
                    lineStart = linkedEntity.mover.Room.middleLeft;
                    lineEnd = linkedEntity.mover.Room.middleRight;
                    break;
                case Room.Half.West:
                    lineStart = linkedEntity.mover.Room.topCenter;
                    lineEnd = linkedEntity.mover.Room.bottomCenter;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Quaternion GetRotation(int index, int totalCount)
        {
            // Magie noire tkt, exemple en mode Third : index * 3 / total = 0, 1 ou 2. 1 est le deuxième tiers donc on l'inverse
            return (index * (int) alternateDirectionMode / totalCount) % 2 == 0
                ? GetSimpleRotation()
                : GetInversedRotation();
        }

        private Quaternion GetSimpleRotation()
        {
            return half switch
            {
                Room.Half.North => Quaternion.Euler(0, 0, 0),
                Room.Half.East => Quaternion.Euler(0, 0, -90),
                Room.Half.South => Quaternion.Euler(0, 0, 180),
                Room.Half.West => Quaternion.Euler(0, 0, 90),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private Quaternion GetInversedRotation()
        {
            return half switch
            {
                Room.Half.North => Quaternion.Euler(0, 0, 180),
                Room.Half.East => Quaternion.Euler(0, 0, 90),
                Room.Half.South => Quaternion.Euler(0, 0, 0),
                Room.Half.West => Quaternion.Euler(0, 0, -90),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public enum AlternateDirectionMode
        {
            Simple = 1,
            Half = 2,
            Third = 3,
            Fourth = 4,
            Sixth = 6,
            Twelfth = 12
        }
    }
}