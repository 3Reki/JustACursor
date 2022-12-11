using System.Collections;
using UnityEngine;

namespace Bosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_Corner", menuName = "Just A Cursor/Pattern/Corner")]
    public class Pat_Corner : Pattern
    {
        private IEnumerator playEnumerator;

        public override void Play(Boss boss)
        {
            base.Play(boss);
            linkedBoss.mover.GoToRandomCorner(patternDuration);
        }

        public override Pattern Stop()
        {
            // TODO Kill Tween
            return base.Stop();
        }
    }
}