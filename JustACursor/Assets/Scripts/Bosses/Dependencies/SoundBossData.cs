using UnityEngine;

namespace Bosses.Dependencies
{
    [CreateAssetMenu(fileName = "SoundBossData", menuName = "Just A Cursor/SoundBossData")]
    public class SoundBossData : BossData
    {
        [SerializeField] public DronesPatternStateMachine[] droneResolver;
    }
}