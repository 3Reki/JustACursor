using System;
using Bosses;
using BulletPro;
using UnityEngine;

namespace Data._Source
{
    [Serializable]
    public class Phase 
    {
        public BossPhase phase;
        public AttackPattern[] attackPatterns = new AttackPattern[1];
    }

    public enum MovementPatternType { None, ToCenter, FarthestFromPlayer, ClosestFromPlayer, DashTowardPlayer, DashAwayFromPlayer }
    public enum TriggerTime { None, BeforeAttack, AfterAttack }
    [Serializable]
    public class MovementPattern
    {
        public MovementPatternType movementPatternType;
        public TriggerTime triggerTime;
        public bool loopTriggerTime;
    }

    [Serializable]
    public class AttackPattern
    {
        public float duration = 1f;
        public EmitterProfile[] emiterProfiles = new EmitterProfile[1];
        public MovementPattern[] movementPatterns = new MovementPattern[1]; // TODO: implement this
    }

    [CreateAssetMenu(fileName = "BossName_BossData", menuName = "Just A Cursor/BossData")]
    public class BossData : ScriptableObject
    {
        [SerializeField] private string bossName;
        [SerializeField, TextArea(10 , 20)] private string description;
        [field: SerializeField] public int startingHP { get; private set; }
        [field: SerializeField, Range(0, 1)] public float phase2HPPercentTrigger { get; private set; } = 0.6f;
        [field: SerializeField, Range(0, 1)] public float phase3HPPercentTrigger { get; private set; } = 0.3f;

        [Space] public Phase[] bossPhases = new Phase[3];
        
        public AttackPattern GetPhasePatterns(BossPhase phase, int patternIndex) => bossPhases[(int)phase].attackPatterns[patternIndex];

        public int GetPatternCountForPhase(BossPhase phase) => bossPhases[(int) phase].attackPatterns.Length;
    }
}