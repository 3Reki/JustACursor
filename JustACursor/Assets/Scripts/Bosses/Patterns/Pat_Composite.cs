using UnityEngine;

namespace Bosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_Composite", menuName = "Just A Cursor/Pattern/Composite Pattern", order = 0)]
    public class Pat_Composite : Pattern<Boss>
    {
        [SerializeField] private Pattern<Boss>[] patterns;
        
        public override void Play(Boss boss)
        {
            base.Play(boss);
            
            foreach (Pattern<Boss> pattern in patterns)
            {
                pattern.Play(boss);
            }
        }

        public override void Update()
        {
            base.Update();
            
            foreach (Pattern<Boss> pattern in patterns)
            {
                pattern.Update();
            }
        }

        public override Pattern<Boss> Stop()
        {
            foreach (Pattern<Boss> pattern in patterns)
            {
                pattern.Stop();
            }
            return base.Stop();
        }
    }
}