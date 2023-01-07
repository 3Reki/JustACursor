using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Just A Cursor/PlayerData", fileName = "Player Data", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [Header("Movement")] public float moveSpeed;
        public AnimationCurve moveAcceleration;
        public AnimationCurve moveDeceleration;
        public AnimationCurve rotationCurve;

        [Header("Dash")] public float dashSpeed;
        [Range(0, 0.5f)] public float dashDuration;
        [Range(0, 0.5f)] public float dashFirstPhaseDuration;
        [Range(0, 5f)] public float dashRefreshCooldown;
        [Range(0, 3)]
        [Tooltip(
            "The strength of the movement (in a different direction than the dash direction), " +
            "only active during the second phase of the dash.")]
        public float dashSecondPhaseControl;
        public float dashShakeIntensity;
        public float dashShakeDuration;
        
        [Header("Shots")] public float fireRate;

        [Header("Time Ability")]
        public float timeAbilityDuration;

        [Header("Health")]
        public int maxHealth;
        public float invinciblityTime;
        public AnimationCurve alphaOscillation;
        public float onHitShakeIntensity;
        public float onHitShakeDuration;

        [Header("Intra UI")]
        public float healthFadeIn;
        public float healthStay;
        public float healthFadeOut;
        public float timeFadeIn;
        public float timeFadeOut;
        
        [Header("Respawn")]
        [Range(0.1f, 1)] public float respawnFadeIn;
        [Range(0.1f, 1)] public float respawnStay;
        [Range(0.1f, 1)] public float respawnFadeOut;
    }
}
