using UnityEngine;

namespace LegacyBosses.Patterns.Drones
{
    [CreateAssetMenu(fileName = "Pat_Dr_Idle", menuName = "Just A Cursor/Pattern/Drones/Idle Pattern", order = 0)]
    public class Pat_Dr_Idle : Pattern<BossSound>
    {
        public override void Stop()
        {
        }
    }
}