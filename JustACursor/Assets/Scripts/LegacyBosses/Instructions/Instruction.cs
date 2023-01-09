using LegacyBosses.Dependencies;
using UnityEngine;

namespace LegacyBosses.Instructions
{
    public abstract class Instruction<T> : ScriptableObject where T : Boss
    {
        [HideInInspector] public InstructionPhase phase = InstructionPhase.None;
        
        [SerializeField] protected Resolver<T> resolver;
        
        protected T linkedEntity;
        
        public virtual void Play(T entity)
        {
            linkedEntity = entity;
            phase = InstructionPhase.Update;
        }

        public abstract void Update();

        public virtual Instruction<T> Stop()
        {
            phase = InstructionPhase.None;
            return resolver.Resolve(linkedEntity);
        }
    }

    public enum InstructionPhase { None, Start, Update, Stop }
}