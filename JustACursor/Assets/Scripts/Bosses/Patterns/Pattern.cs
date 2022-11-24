using UnityEngine;

namespace Bosses.Patterns
{
    
    public abstract class Pattern : ScriptableObject
    {
        [SerializeField] protected Resolver resolver;
        [SerializeField, Range(0, 30f)] protected float patternDuration = 3f;
        
        protected Boss linkedBoss;
        protected float currentPatternTime;
        
        public virtual Pattern Play(Boss boss)
        {
            linkedBoss = boss;
            currentPatternTime = patternDuration;
            return this;
        }

        public virtual Pattern Update()
        {
            currentPatternTime -= Time.deltaTime;

            return currentPatternTime < 0 ? Stop() : this;
        }

        public virtual Pattern Stop()
        {
            return resolver.Resolve(linkedBoss);
        }
    }
}