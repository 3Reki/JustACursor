using System;
using System.Collections.Generic;
using Bosses.Conditions;
using Bosses.Patterns;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bosses
{
    //[CreateAssetMenu(fileName = "Resolver", menuName = "Just A Cursor/Pattern Resolver", order = 0)]
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

            return selectedList.Count == 0 
                ? null 
                : selectedList[Random.Range(0, selectedList.Count)].pattern.Play(boss);
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
                    ConditionType.Test => cdtTest,
                    ConditionType.None => cdtNone,
                    _ => null
                };
            }
        }
        
        public Pattern pattern;
        [Range(0, 20)]
        public float weight;

        [SerializeField] private Cdt_None cdtNone;
        [SerializeField] private Cdt_HealthThreshold cdtHealthThreshold;
        [SerializeField] private Cdt_Test cdtTest;
    }
}