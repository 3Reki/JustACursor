using UnityEngine;

namespace Bosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_Center", menuName = "Just A Cursor/Pattern/Center")]
    public class Pat_Center : Pattern<Boss>
    {
        public override void Play(Boss boss)
        {
            base.Play(boss);
            linkedBoss.mover.GoToCenter(patternDuration);
        }

        public override Pattern<Boss> Stop()
        {
            // TODO Kill Tween
            return base.Stop();
        }
    }
}