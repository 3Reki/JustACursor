using System;
using UnityEngine;

namespace Bosses.Conditions
{
    public enum ConditionType
    {
        None,
        HealthThreshold
    }
    
    public interface ICondition
    {
        public bool Check(Boss boss); // LéBossó
    }
}