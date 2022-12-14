using UnityEngine;

namespace Bosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_LookAtPlayer", menuName = "Just A Cursor/Pattern/Look At Player")]
    public class Pat_LookAtPlayer : Pattern<Boss>
    {
        public override void Update()
        {
            base.Update();
            
            linkedEntity.mover.RotateTowardsPlayer();
        }

        public override void Stop() {
            base.Stop();
        }
    }
}