using System;
using Bosses.Conditions;
using UnityEngine;

namespace Bosses.Dependencies
{
    [Serializable]
    public class ResolvedPattern
    {
        public ConditionType conditionType;

        public ICondition condition
        {
            get
            {
                return conditionType switch
                {
                    ConditionType.HealthThreshold => cdtHealthThreshold,
                    ConditionType.CornerDistance => cdtCornerDistance,
                    ConditionType.CenterDistance => cdtCenterDistance,
                    ConditionType.BossDistance => cdtBossDistance,
                    ConditionType.Quarter => cdtQuarter,
                    ConditionType.Half => cdtHalf,
                    ConditionType.None => cdtNone,
                    _ => null
                };
            }
        }

        [Range(0, 20)] public float weight = 1f;

        [SerializeField] private Cdt_None cdtNone;
        [SerializeField] private Cdt_HealthThreshold cdtHealthThreshold;
        [SerializeField] private Cdt_CornerDistance cdtCornerDistance;
        [SerializeField] private Cdt_CenterDistance cdtCenterDistance;
        [SerializeField] private Cdt_BossDistance cdtBossDistance;
        [SerializeField] private Cdt_Quarter cdtQuarter;
        [SerializeField] private Cdt_Half cdtHalf;
    }
    
    public enum ConditionType
    {
        None,
        HealthThreshold,
        CornerDistance,
        CenterDistance,
        BossDistance,
        Quarter,
        Half
    }
}