using UnityEngine;

namespace LegacyBosses.Dependencies
{
    public class SoundBossData : BossData
    {
        [SerializeField] public Resolver<BossSound>[] droneResolvers;
    }
}