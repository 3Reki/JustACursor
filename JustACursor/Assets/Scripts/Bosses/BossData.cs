using UnityEngine;

namespace Bosses
{
    [CreateAssetMenu(fileName = "BossName_BossData", menuName = "Just A Cursor/BossData")]
    public class BossData : ScriptableObject
    {
        [SerializeField] private string bossName;
        [SerializeField, TextArea(10 , 20)] private string description;
        [SerializeField] public int startingHP;
        [SerializeField, Range(0, 1)] public float phase2HPPercentTrigger = 0.6f;
        [SerializeField, Range(0, 1)] public float phase3HPPercentTrigger = 0.3f;
        [SerializeField] public Phase[] phases;
        [SerializeField] public float patternCooldown;
    }
}