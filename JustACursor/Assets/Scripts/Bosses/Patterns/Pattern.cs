using UnityEngine;

namespace Bosses.Patterns
{
    
    public abstract class Pattern : ScriptableObject
    {
        [SerializeField] protected Resolver resolver;
        [SerializeField, Range(0, 30f)] protected float patternDuration = 3f;
        
        protected Boss linkedBoss;
        protected float currentPatternTime;
        
        public virtual void Play(Boss boss)
        {
            linkedBoss = boss;
            currentPatternTime = patternDuration;
            boss.currentPatternPhase = PatternPhase.Update;
        }

        public virtual void Update()
        {
            currentPatternTime -= Time.deltaTime;

            if (currentPatternTime < 0)
            {
                linkedBoss.currentPatternPhase = PatternPhase.Stop;
            }
        }

        public virtual Pattern Stop()
        {
            return resolver.Resolve(linkedBoss);
        }
    }
}