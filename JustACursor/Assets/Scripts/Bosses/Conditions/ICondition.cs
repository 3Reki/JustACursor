using System;
using UnityEngine;

namespace Bosses.Conditions
{
    public enum ConditionType
    {
        None,
        HealthThreshold,
        Test
    }
    
    public interface ICondition
    {
        public bool Check(Boss boss);
    }
}