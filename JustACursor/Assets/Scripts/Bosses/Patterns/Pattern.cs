using Graph;
using UnityEngine;

namespace Bosses.Patterns
{
    [NodeWidth(300)]
    public abstract class Pattern<T> : BaseNode
    {
        public State currentState { get; protected set; } = State.Start;
        
        [Input(connectionType = ConnectionType.Override)] public int entry;
        [Output(connectionType = ConnectionType.Override)] public int exit;
        
        [SerializeField, Range(0, 30f)] protected float patternDuration = 3f;

        protected T linkedEntity;
        protected float currentPatternTime;

        private void OnDisable()
        {
            currentState = State.Start;
        }

        public virtual void Play(T entity)
        {
            linkedEntity = entity;
            currentPatternTime = patternDuration;
            currentState = State.Update;
        }

        public virtual void Update()
        {
            currentPatternTime -= Time.deltaTime * Energy.GameSpeed;

            if (currentPatternTime < 0)
            {
                currentState = State.Stop;
            }
        }

        public virtual void Stop()
        {
            currentState = State.Start;
        }
        
        public enum State
        {
            Start,
            Update,
            Stop
        }
    }
}