using System.Collections;
using UnityEngine;

namespace Bosses.Instructions.Patterns
{
    [CreateAssetMenu(fileName = "Pat_Corner", menuName = "Just A Cursor/Pattern/Corner")]
    public class Pat_Corner : Pattern<Boss>
    {
        private IEnumerator playEnumerator;

        public override void Play(Boss boss)
        {
            base.Play(boss);
            linkedEntity.mover.GoToRandomCorner(patternDuration);
        }

        public override void Stop()
        {
            // TODO Kill Tween
        }
    }
}