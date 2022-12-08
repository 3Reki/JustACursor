using System.Collections;
using UnityEngine;

namespace Bosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_Corner", menuName = "Just A Cursor/Pattern/Corner")]
    public class Pat_Corner : Pattern<Boss>
    {
        private IEnumerator playEnumerator;

        public override void Play(Boss boss)
        {
            base.Play(boss);
            linkedBoss.mover.GoToRandomCorner(patternDuration);
        }

        public override Pattern<Boss> Stop()
        {
            // TODO Kill Tween
            return base.Stop();
        }
    }
}