using System;

namespace Bosses.Conditions
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