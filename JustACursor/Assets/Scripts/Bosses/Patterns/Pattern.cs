using UnityEngine;

namespace Bosses.Patterns
{
    
    public abstract class Pattern : ScriptableObject
    {
        [field: SerializeField] public float length { get; protected set; }
        
        protected Boss boss;

        public void SetTargetBoss(Boss target)
        {
            boss = target;
        }
        
        public abstract void Play();

        public abstract void Stop();
    }
}