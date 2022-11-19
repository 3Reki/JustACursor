using BulletPro;
using System;
using UnityEngine;

[Serializable]
public class AttackPattern
{
    public BossPhase phase;
    public EmitterProfile[] patterns = new EmitterProfile[3];
}

[CreateAssetMenu(fileName = "BossName_BossData", menuName = "Just A Cursor/BossData")]
public class BossData : ScriptableObject
{
    [SerializeField] private string bossName;
    [SerializeField, TextArea(10 , 20)] private string description;
    [SerializeField] private int startingHP = 800; 
    [SerializeField, Range(0, 1)] private float phase2HPPercentTrigger = 0.6f;
    [SerializeField, Range(0, 1)] private float phase3HPPercentTrigger = 0.3f;

    public AttackPattern[] attackPattern = new AttackPattern[1];
    public BulletEmitter[] BulletEmitter { get; set; }

    public int CurrentHP { get; set; }
    public BossPhase CurrentBossPhase { get; set; }



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
        GoToPhase(BossPhase.One); 
    }

    /// <summary>
    /// This function changes the profile of the bullet emitter without destroying already instantiated bullets
    /// </summary>
    public void GoToPhase(BossPhase phase)
    {
        for (int i = 0; i < BulletEmitter.Length; i++)
        {
            BulletEmitter[i].Stop();
            if (i < attackPattern[(int)phase].patterns.Length)
            {
                BulletEmitter[i].Play();
                BulletEmitter[i].SwitchProfile(attackPattern[(int)phase].patterns[i]);
            }
        }
    }

    // just because I want to keep the percent trigger at the boss data level and NOT in the EmitterControllers
    public bool CheckPhase2HPTrigger() => (float)CurrentHP / startingHP <= phase2HPPercentTrigger;     

    public bool CheckPhase3HPTrigger() => (float)CurrentHP / startingHP <= phase3HPPercentTrigger;

    public AttackPattern GetPhasePatterns(BossPhase phase) => attackPattern[(int)phase];
}
