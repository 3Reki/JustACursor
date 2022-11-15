using BulletPro;
using System;
using UnityEngine;

[Serializable]
public class Phase 
{
    public BossPhase phase;
    public AttackPattern[] attackPatterns = new AttackPattern[1];
}

public enum MovementPatternType { NONE, ToCenter, FarthestFromPlayer, ClosestFromPlayer, DashTowardPlayer, DashAwayFromPlayer }
public enum TriggerTime { NONE, BeforeAttack, AfterAttack }
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
    public EmitterProfile[] emiterProfiles = new EmitterProfile[3];
    public MovementPattern[] movementPatterns = new MovementPattern[1]; // TODO: implement this
}

[CreateAssetMenu(fileName = "BossName_BossData", menuName = "Just A Cursor/BossData")]
public class BossData : ScriptableObject
{
    [SerializeField] private string bossName;
    [SerializeField, TextArea(10 , 20)] private string description;
    [SerializeField] private int startingHP = 800; 
    [SerializeField, Range(0, 1)] private float phase2HPPercentTrigger = 0.6f;
    [SerializeField, Range(0, 1)] private float phase3HPPercentTrigger = 0.3f;

    [Space] public Phase[] bossPhases = new Phase[3]; 
    
    public BulletEmitter[] BulletEmitter { get; set; }
    public int CurrentHP { get; set; }
    public BossPhase CurrentBossPhase { get; set; }
    public BossPhase PreviousBossPhase { get; set; }

    // ALL THE LOGIC SHOULD BE IN THE FSM, NOT HERE (only data)

    /// <summary>
    /// This function initializes the BossData with it's emitters and starting HP
    /// </summary>
    public void Init(BulletEmitter[] emitters)
    {
        CurrentHP = startingHP;
        BulletEmitter = new BulletEmitter[emitters.Length];
        Array.Copy(emitters, BulletEmitter, emitters.Length); 
    }

    public void StartPhaseOne()
    {
        GoToPhase(BossPhase.One, 0); 
    }

    /// <summary>
    /// This function changes the profile of the bullet emitter without destroying already instantiated bullets
    /// </summary>
    public void GoToPhase(BossPhase phase, int patternIndex)
    {
        for (int i = 0; i < bossPhases[(int)phase].attackPatterns[patternIndex].emiterProfiles.Length; i++)
        {
            BulletEmitter[i].SwitchProfile(bossPhases[(int)phase].attackPatterns[patternIndex].emiterProfiles[i]);
        }
    }

    // just because I want to keep the percent trigger at the boss data level and NOT in the EmitterControllers
    public bool CheckPhase2HPTrigger() => (float)CurrentHP / startingHP <= phase2HPPercentTrigger;     

    public bool CheckPhase3HPTrigger() => (float)CurrentHP / startingHP <= phase3HPPercentTrigger;

    public AttackPattern GetPhasePatterns(BossPhase phase, int patternIndex) => bossPhases[(int)phase].attackPatterns[patternIndex];
}
