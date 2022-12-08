using Bosses.Dependencies;
using UnityEngine;

namespace Bosses.Patterns
{
    
    public abstract class Pattern<T> : ScriptableObject where T : Boss
    {
        [HideInInspector] public PatternPhase phase = PatternPhase.None;
        [SerializeField] protected Resolver<T> resolver;
        [SerializeField, Range(0, 30f)] protected float patternDuration = 3f;
        
        protected T linkedBoss;
        protected float currentPatternTime;
        
        public virtual void Play(T boss)
        {
            linkedBoss = boss;
            currentPatternTime = patternDuration;
            phase = PatternPhase.Update;
        }

        public virtual void Update()
        {
            currentPatternTime -= Time.deltaTime;

            if (currentPatternTime < 0)
            {
                phase = PatternPhase.Stop;
            }
        }

        public virtual Pattern<T> Stop()
        {
            phase = PatternPhase.None;
            return resolver.Resolve(linkedBoss);
        }
    }
    
    public enum PatternPhase { None, Start, Update, Stop }
}