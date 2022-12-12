using LD;
using UnityEngine;

namespace Bosses.Instructions.Patterns
{
    [CreateAssetMenu(fileName = "Pat_Corner", menuName = "Just A Cursor/Pattern/Corner")]
    public class Pat_Corner : Pattern<Boss>
    {
        [SerializeField] private Room.Corner corner;

        public override void Play(Boss entity)
        {
            base.Play(entity);
            linkedEntity.mover.GoToCorner(corner, patternDuration);
        }

        public override void Stop()
        {
            // TODO Kill Tween
        }
    }
}