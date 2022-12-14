using LegacyBosses.Patterns;
using UnityEngine;

namespace LegacyBosses.Instructions
{
    public class InstructionSimple<T> : Instruction<T> where T : Boss
    {
        [SerializeField] private Pattern<T> pattern;
        
        public override void Play(T entity)
        {
            base.Play(entity);
            pattern.Play(entity);
        }

        public override void Update()
        {
            pattern.Update();

            if (pattern.isFinished)
            {
                phase = InstructionPhase.Stop;
            }
        }
    }
}