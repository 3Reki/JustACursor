using System;

namespace LegacyBosses.Conditions
{
    [Serializable]
    public struct Cdt_None : ICondition
    {
        public bool Check(Boss boss)
        {
            return true;
        }
    }
}