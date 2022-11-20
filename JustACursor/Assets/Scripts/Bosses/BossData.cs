using UnityEngine;

namespace Bosses
{
    [CreateAssetMenu(fileName = "BossName_BossData", menuName = "Just A Cursor/BossData")]
    public class BossData : ScriptableObject
    {
        [SerializeField] private string bossName;
        [SerializeField, TextArea(10 , 20)] private string description;
        [field: SerializeField] public int startingHP { get; private set; }
        [field: SerializeField, Range(0, 1)] public float phase2HPPercentTrigger { get; private set; } = 0.6f;
        [field: SerializeField, Range(0, 1)] public float phase3HPPercentTrigger { get; private set; } = 0.3f;
    }
}