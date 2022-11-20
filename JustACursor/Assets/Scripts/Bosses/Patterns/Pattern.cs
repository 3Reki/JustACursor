using UnityEngine;

namespace Bosses.Patterns
{
    
    public abstract class Pattern : ScriptableObject
    {
        [field: SerializeField] public float length { get; protected set; }
        
        protected BossVirus boss;

        public void SetTargetBoss(BossVirus target)
        {
            boss = target;
        }
        
        public abstract void Play();

        public abstract void Stop();
    }
}