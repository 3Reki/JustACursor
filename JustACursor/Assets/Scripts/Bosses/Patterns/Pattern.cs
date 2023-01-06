using Graph;
using UnityEngine;

namespace Bosses.Patterns
{
    [NodeWidth(300)]
    public abstract class Pattern<T> : BaseNode
    {
        [Input] public int entry;
        [Output] public int exit;
        public bool isFinished { get; protected set; }
        
        [SerializeField, Range(0, 30f)] protected float patternDuration = 3f;

        protected T linkedEntity;
        protected float currentPatternTime;
        
        public virtual void Play(T entity)
        {
            linkedEntity = entity;
            currentPatternTime = patternDuration;
            isFinished = false;
        }

        public virtual void Update()
        {
            currentPatternTime -= Time.deltaTime * Energy.GameSpeed;

            if (currentPatternTime < 0)
            {
                isFinished = true;
            }
        }

        public abstract void Stop();
    }
}