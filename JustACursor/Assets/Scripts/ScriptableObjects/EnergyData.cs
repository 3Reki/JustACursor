using DG.Tweening;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Just A Cursor/EnergyData", fileName = "Energy Data")]
    public class EnergyData : ScriptableObject
    {
        [Header("Lerp Parameters")]
        [Range(1, 4)]
        public float speedUpModifier;
        [Range(0, 1)]
        public float slowDownModifier;
        public float lerpDuration;
        public Ease lerpEase;
    }
}
