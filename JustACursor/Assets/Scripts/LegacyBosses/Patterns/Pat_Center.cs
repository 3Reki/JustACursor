using UnityEngine;

namespace LegacyBosses.Patterns
{
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