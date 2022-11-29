using UnityEngine;

namespace Bosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_LookAtPlayer", menuName = "Just A Cursor/Pattern/Look At Player")]
    public class Pat_LookAtPlayer : Pattern
    {
        public override void Update()
        {
            base.Update();
            
            linkedBoss.mover.RotateTowardsPlayer();
        }

    }
}