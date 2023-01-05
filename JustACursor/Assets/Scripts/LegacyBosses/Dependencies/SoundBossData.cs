using UnityEngine;

namespace LegacyBosses.Dependencies
{
    [CreateAssetMenu(fileName = "SoundBossData", menuName = "Just A Cursor/SoundBossData")]
    public class SoundBossData : BossData
    {
        [SerializeField] public Resolver<BossSound>[] droneResolvers;
    }
}