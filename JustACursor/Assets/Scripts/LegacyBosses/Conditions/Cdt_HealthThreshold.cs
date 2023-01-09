using System;
using UnityEngine;

namespace LegacyBosses.Conditions
{
    [Serializable]
    public struct Cdt_HealthThreshold : ICondition
    {
        [SerializeField] private ComparisonType comparisonType;
        [SerializeField, Range(0f, 1f)] private float threshold;
        
        public bool Check(Boss boss)
        {
            float currentPercentHP = (float) boss.Health.CurrentHealth / boss.maxHP;
            return comparisonType switch
            {
                ComparisonType.InferiorTo => currentPercentHP < threshold,
                ComparisonType.InferiorOrEqual => currentPercentHP <= threshold,
                ComparisonType.SuperiorOrEqual => currentPercentHP >= threshold,
                ComparisonType.Superior => currentPercentHP > threshold,
                ComparisonType.Equal => Math.Abs(currentPercentHP - threshold) < 0.005,
                _ => throw new ArgumentOutOfRangeException()
            };
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