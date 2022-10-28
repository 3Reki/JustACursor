using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Just A Cursor", fileName = "Player Data", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [Header("Movement")] public float moveSpeed;
        public AnimationCurve moveAcceleration;
        public AnimationCurve moveDeceleration;
        [Range(0, 1)] public float rotationSpeed;

        [Header("Dash")] public float dashSpeed;
        [Range(0, 0.5f)] public float dashDuration;
        [Range(0, 0.5f)] public float dashFirstPhaseDuration;
        [Range(0, 5f)] public float dashRefreshCooldown;

        [Range(0, 3)]
        [Tooltip(
            "The strength of the movement (in a different direction than the dash direction), " +
            "only active during the second phase of the dash.")]
        public float dashSecondPhaseControl;

        [Header("Shots")] public float fireRate;

        [Header("Time Ability")] public float timeAbilityDuration;
        [Range(1, 4)] public float speedUpModifier;
        [Range(0, 1)] public float speedDownModifier;
    }
}