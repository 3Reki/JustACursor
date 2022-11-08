using BulletPro;
using UnityEngine;
using TMPro;

public enum BossPhase { NONE = -1, One, Two, Three }
// this is a simple class to set up boss logic
// the heavy part of logic is done on the BossData and called here via enum-based state machine
// this class should NOT be in charge of boss-specific mechanics

/// <summary>
/// This is a simple class to set up boss logic. The heavy part of logic is done on the BossData and called here via enum-based state machine.
/// Said otherwise, this class is about WHAT and WHEN while BossData is about HOW
/// </summary>
public class BasicBossFSM : MonoBehaviour
{
    [SerializeField] BossData bossData;
    [SerializeField] TMP_Text bossHP;
    [SerializeField] private BulletEmitter[] emmitters = new BulletEmitter[3]; // must be used only for init

    [Header("DEBUG")]
    [Tooltip("0 means don't play the pattern at this index. 1 means play it;")] public int[] playMask = { 1, 1, 1 };
    [SerializeField] private bool overrideOnStart = false;
    [SerializeField] private BossPhase phaseOverride = BossPhase.One;
    [SerializeField] private KeyCode doPhaseOverride = KeyCode.Space;
    [SerializeField] private KeyCode freeze = KeyCode.F;
    [SerializeField] private KeyCode refresh = KeyCode.R;

    bool isPaused;

    private void Start()
    {
        bossData.Init(emmitters);
        bossHP.text = $"{bossData.CurrentHP}";
        SetBossPhase(overrideOnStart ? phaseOverride : BossPhase.One); 
    }

    void Update()
    {
        // change to new profile
        if (Input.GetKeyDown(doPhaseOverride))
        {
            SetBossPhase(phaseOverride);
        }

        // freeze effect. Placeholder
        if (Input.GetKeyDown(freeze))
        {
            FreezeBossBullets();
        }

        if (Input.GetKeyDown(refresh))
        {
            Debug_RefreshPlayID(); 
        }
    } 

    private void FreezeBossBullets()
    {
        isPaused = !isPaused;

        for (int i = 0; i < bossData.attackPattern.Length; i++)
        {
            if (isPaused) bossData.BulletEmitter[i].Pause(PlayOptions.AllBullets);
            else bossData.BulletEmitter[i].Play(PlayOptions.AllBullets);
        }
    }

    private void Debug_RefreshPlayID()
    {
        for (int i = 0; i < bossData.attackPattern[(int)bossData.CurrentBossPhase].patterns.Length; i++)
        {
            bossData.BulletEmitter[i].Stop(); // BAD : will throw an error if emitters are not filld in order
            if (playMask[i] == 1)
            {
                bossData.BulletEmitter[i].Play();
                Debug.Log("playing pattern: " + bossData.attackPattern[(int)bossData.CurrentBossPhase].patterns[i]);
            }
        }
    }

    // PLACEHOLDER
    // REFACTORING: should be moved elsewere
    public void TakeDamage(BulletPro.Bullet bullet, Vector3 hitPoint)
    {
        bossData.CurrentHP -= bullet.moduleParameters.GetInt("Damage");
        bossHP.text = $"{bossData.CurrentHP}"; 

        switch (bossData.CurrentBossPhase)
        {
            case BossPhase.NONE:
                Debug.LogError("Boss Phase is NONE");
                break;
            case BossPhase.One:
                if (bossData.CheckPhase2HPTrigger()) SetBossPhase(BossPhase.Two);
                break;
            case BossPhase.Two:
                if (bossData.CheckPhase3HPTrigger()) SetBossPhase(BossPhase.Three);
                break;
            case BossPhase.Three:
                if (bossData.CurrentHP <= 0) KillBoss();
                break;
        }
    }

    private void SetBossPhase(BossPhase newPhase)
    {
        bossData.CurrentBossPhase = newPhase;

        SetNewPatterns(); 
    }

    private void SetNewPatterns()
    {
        bossData.GoToPhase(bossData.CurrentBossPhase);
        Debug.Log("setting patterns to new phase: " + bossData.CurrentBossPhase);

        Debug_RefreshPlayID();
    }

    // PLACEHOLDER
    // BAD : no need to go through all the emitters
    // REFACTORING: should be moved elsewere (same class as  TakeDamage())
    private void KillBoss()
    {
        Debug.Log("Boss is killed");
        for (int i = 0; i < bossData.attackPattern.Length; i++)
        {
            bossData.BulletEmitter[i].Stop();
        }

        transform.root.gameObject.SetActive(false);
    }
}
