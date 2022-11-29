using UnityEngine;

namespace Bosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_Composite", menuName = "Just A Cursor/Pattern/Composite Pattern", order = 0)]
    public class Pat_Composite : Pattern
    {
        [SerializeField] private Pattern[] patterns;
        
        public override void Play(Boss boss)
        {
            base.Play(boss);
            
            foreach (Pattern pattern in patterns)
            {
                pattern.Play(boss);
            }
        }

        public override void Update()
        {
            base.Update();
            
            foreach (Pattern pattern in patterns)
            {
                pattern.Update();
            }
        }

        public override Pattern Stop()
        {
            foreach (Pattern pattern in patterns)
            {
                pattern.Stop();
            }
            return base.Stop();
        }
    }
}