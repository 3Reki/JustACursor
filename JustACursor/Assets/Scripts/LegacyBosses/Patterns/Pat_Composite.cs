using UnityEngine;

namespace LegacyBosses.Patterns
{
    public class Pat_Composite : Pattern<Boss>
    {
        [SerializeField] private Pattern<Boss>[] patterns;
        
        public override void Play(Boss entity)
        {
            base.Play(entity);
            
            foreach (Pattern<Boss> pattern in patterns)
            {
                pattern.Play(entity);
            }
        }

        public override void Update()
        {
            foreach (Pattern<Boss> pattern in patterns)
            {
                pattern.Update();
            }

            foreach (Pattern<Boss> pattern in patterns)
            {
                if (!pattern.isFinished)
                    return;
            }

            isFinished = true;
        }

        public override void Stop()
        {
            foreach (Pattern<Boss> pattern in patterns)
            {
                pattern.Stop();
            }
        }
    }
}