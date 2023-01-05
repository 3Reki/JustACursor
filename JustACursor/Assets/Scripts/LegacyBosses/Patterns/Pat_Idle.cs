using UnityEngine;

namespace LegacyBosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_Idle", menuName = "Just A Cursor/Pattern/Idle Pattern", order = 0)]
    public class Pat_Idle : Pattern<Boss>
    {
        public override void Stop()
        {
        }
    }
}