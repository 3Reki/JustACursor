using System;
using Bosses.Conditions;
using Bosses.Patterns;
using UnityEngine;

namespace Bosses
{
    [CreateAssetMenu(fileName = "Resolver", menuName = "Just A Cursor/Pattern Resolver", order = 0)]
    public class Resolver : ScriptableObject
    {
        [SerializeField] private ResolvedPattern choices;

        public Pattern Resolve(Boss boss)
        {
            // foreach (ResolvedPattern choice in choices)
            // {
            //     if (choice.cdtHealthThreshold.Check(boss))
            //     {
            //         return choice.pattern;
            //     }
            // }

            return null;
        }
    }

    [Serializable]
    public struct ResolvedPattern
    {
        public ConditionType conditionType;
        public Pattern pattern;
        [Range(0, 20)]
        public float weight;
        
        public Cdt_HealthThreshold cdtHealthThreshold;
    }
}