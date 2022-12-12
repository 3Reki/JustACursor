using UnityEngine;

namespace Bosses.Instructions.Patterns
{
    [CreateAssetMenu(fileName = "Pat_Center", menuName = "Just A Cursor/Pattern/Center")]
    public class Pat_Center : Pattern<Boss>
    {
        public override void Play(Boss entity)
        {
            base.Play(entity);
            linkedEntity.mover.GoToCenter(patternDuration);
        }

        public override void Stop()
        {
            // TODO Kill Tween
        }
    }
}