using UnityEngine;

namespace LegacyBosses.Dependencies
{
    public class BossData : ScriptableObject
    {
        [SerializeField] private string bossName;
        [SerializeField, TextArea(10 , 20)] private string description;
        [SerializeField] public int startingHP;
        [SerializeField, Range(0, 1)] public float phase2HPThreshold = 0.6f;
        [SerializeField, Range(0, 1)] public float phase3HPThreshold = 0.3f;
        [SerializeField] public Resolver<Boss>[] phaseResolvers;
    }
}