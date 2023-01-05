using LegacyBosses.Patterns;
using UnityEngine;

namespace LegacyBosses.Instructions
{
    
    public class InstructionComposite<T> : Instruction<T> where T : Boss
    {
        [SerializeField] private Pattern<T>[] patterns;
        
        public override void Play(T entity)
        {
            base.Play(entity);
            
            foreach (Pattern<T> pattern in patterns)
            {
                pattern.Play(entity);
            }
        }

        public override void Update()
        {
            foreach (Pattern<T> pattern in patterns)
            {
                pattern.Update();
            }

            foreach (Pattern<T> pattern in patterns)
            {
                if (!pattern.isFinished)
                    return;
            }

            phase = InstructionPhase.Stop;
        }

        public override Instruction<T> Stop()
        {
            foreach (Pattern<T> pattern in patterns)
            {
                pattern.Stop();
            }
            
            return base.Stop();
        }
    }
}