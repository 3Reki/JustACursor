using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Just A Cursor", fileName = "Player Data", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [Header("Movement")] 
        public float moveSpeed;
        public AnimationCurve moveAcceleration;
        [Range(0, 1)]
        public float rotationSpeed;
        
        [Header("Dash")]
        public AnimationCurve dashSpeed;
        [Range(0, 0.5f)]
        public float dashDuration;
        [Range(0, 5f)]
        public float dashRefreshCooldown;

        [Header("Shots")] 
        public float fireRate;

        [Header("Time Ability")] 
        public float timeAbilityDuration;
        [Range(1, 4)]
        public float speedUpModifier;
        [Range(0, 1)]
        public float speedDownModifier;
    }
}
