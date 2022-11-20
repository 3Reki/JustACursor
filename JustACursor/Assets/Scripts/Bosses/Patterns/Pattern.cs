using UnityEngine;

namespace Bosses.Patterns
{
    [RequireComponent(typeof(Boss))]
    public abstract class Pattern : MonoBehaviour
    {
        [SerializeField] protected BossVirus boss;
        [field: SerializeField] public float length { get; protected set; }
        
        public abstract void Play();

        public abstract void Stop();
    }
}