using System.Collections.Generic;
using Graph;

namespace Bosses.Dependencies
{
    public class Resolver<T> : BaseNode where T : Boss
    {
        [Input(ShowBackingValue.Never)] public int entry;

        [Output(dynamicPortList = true, connectionType = ConnectionType.Override)]
        public ResolvedPattern[] choices;

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
}