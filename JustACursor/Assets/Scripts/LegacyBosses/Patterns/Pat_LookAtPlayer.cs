using UnityEngine;

namespace LegacyBosses.Patterns
{
    public class Pat_LookAtPlayer : Pattern<Boss>
    {
        public override void Update()
        {
            base.Update();
            
            linkedEntity.mover.RotateTowardsPlayer();
        }

        public override void Stop() {}
    }
}