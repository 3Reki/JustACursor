using System;
using System.Collections.Generic;
using Bosses.Conditions;
using Bosses.Patterns;
using UnityEngine;

namespace Bosses
{
    [Serializable]
    public class Resolver
    {
        [SerializeField] private ResolvedPattern[] choices;

        private readonly List<ResolvedPattern> selectedList = new();
        
        public Pattern Resolve(Boss boss)
        {
            selectedList.Clear();
            
            foreach (ResolvedPattern choice in choices)
            {
                if (choice.condition.Check(boss))
                {
                    selectedList.Add(choice); 
                }
            }

            if (selectedList.Count == 0)
            {
                boss.currentPatternPhase = PatternPhase.None;
                return null;
            }

            boss.currentPatternPhase = PatternPhase.Start;
            return selectedList.RandomWeightedSelection().pattern;
        }
    }

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
        
        public Pattern pattern;
        [Range(0, 20)]
        public float weight = 1;

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