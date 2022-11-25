using System;
using UnityEngine;

namespace Bosses.Conditions
{
    [Serializable]
    public struct Cdt_HealthThreshold : ICondition
    {
        [SerializeField] private ComparisonType comparisonType;
        [SerializeField, Range(0f, 1f)] private float threshold;
        
        public bool Check(Boss boss)
        {
            return (float) boss.currentHp / boss.maxHP < threshold;
        }
        
        public enum ComparisonType
        {
            InferiorTo,
            InferiorOrEqual,
            SuperiorOrEqual,
            Superior,
            Equal
        }
    }
    
    [Serializable]
    public struct Cdt_Test : ICondition
    {
        [SerializeField] private ComparisonType comparisonType;
        [SerializeField] private bool cond;
        
        public bool Check(Boss boss)
        {
            return cond;
        }
        
        public enum ComparisonType
        {
            InferiorTo,
            InferiorOrEqual,
            SuperiorOrEqual,
            Superior,
            Equal
        }
    }
}