using System;
using System.Collections.Generic;
using Bosses.Conditions;
using Graph;
using UnityEngine;

namespace Bosses.Dependencies
{
    public class Resolver<T> : BaseNode where T : Boss
    {
        [Input(ShowBackingValue.Never)] public int entry;

        [Output(dynamicPortList = true, connectionType = ConnectionType.Override)] 
        [SerializeField] private ResolvedPattern[] choices;

        private readonly List<ResolvedPattern> selectedList = new();

        public int Resolve(T boss)
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
                return -1;
            }

            int i = selectedList.RandomWeightedSelection();
            //instruction.phase = InstructionPhase.Start;
            return i;
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