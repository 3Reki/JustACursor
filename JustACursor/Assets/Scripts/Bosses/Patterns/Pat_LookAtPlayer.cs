using UnityEngine;

namespace Bosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_LookAtPlayer", menuName = "Just A Cursor/Pattern/Look At Player")]
    public class Pat_LookAtPlayer : Pattern
    {
        public override Pattern Update()
        {
            linkedBoss.mover.RotateTowardsPlayer();
            return base.Update();
        }

    }
}