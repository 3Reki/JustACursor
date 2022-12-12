using System;
using System.Collections.Generic;
using Bosses.Conditions;
using Bosses.Instructions;
using UnityEngine;

namespace Bosses.Dependencies
{
    [Serializable]
    public class Resolver<T> where T : Boss
    {
        [SerializeField] private ResolvedPattern<T>[] choices;

        private readonly List<ResolvedPattern<T>> selectedList = new();
        
        public Instruction<T> Resolve(T boss)
        {
            selectedList.Clear();
            
            foreach (ResolvedPattern<T> choice in choices)
            {
                if (choice.condition.Check(boss))
                {
                    selectedList.Add(choice); 
                }
            }

            if (selectedList.Count == 0)
            {
                return null;
            }

            Instruction<T> instruction = selectedList.RandomWeightedSelection().pattern;
            instruction.phase = InstructionPhase.Start;
            return instruction;
        }
    }

    [Serializable]
    public class ResolvedPattern<T> where T : Boss
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
        
        public Instruction<T> pattern;
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