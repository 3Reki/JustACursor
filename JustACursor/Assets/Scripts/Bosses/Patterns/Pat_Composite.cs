using BulletPro;
using UnityEngine;

namespace Bosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_Composite", menuName = "Just A Cursor/Pattern/Composite Pattern", order = 0)]
    public class Pat_Composite : Pattern
    {
        [SerializeField] private Pattern[] patterns;
        
        public override Pattern Play(Boss boss)
        {
            // TODO
            return base.Play(boss);
        }

        public override Pattern Stop()
        {
            
            return base.Stop();
        }
    }
}