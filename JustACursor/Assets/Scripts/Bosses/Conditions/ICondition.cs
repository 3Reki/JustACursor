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
    
    [Serializable]
    public class ICondition
    {
        public virtual bool Check(Boss boss)
        {
            return false;
        } 
    }
}