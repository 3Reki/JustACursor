using System;
using LD;
using UnityEngine;

namespace Bosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_Border", menuName = "Just A Cursor/Pattern/Border Pattern", order = 0)]
    public class Pat_Border : Pattern<Boss>
    {
        [SerializeField] private Room.Half border;
        
        public override void Play(Boss entity)
        {
            base.Play(entity);
            linkedEntity.mover.GoToBorder(border, patternDuration);
        }

        public override void Stop()
        {
            base.Stop();
            switch (border)
            {
                case Room.Half.North:
                    linkedEntity.mover.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case Room.Half.East:
                    linkedEntity.mover.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case Room.Half.South:
                    linkedEntity.mover.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case Room.Half.West:
                    linkedEntity.mover.transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            // TODO Kill Tween
        }
    }
}