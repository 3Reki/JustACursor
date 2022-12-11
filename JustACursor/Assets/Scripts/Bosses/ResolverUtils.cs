using System.Collections.Generic;
using UnityEngine;

namespace Bosses
{
    public static class ResolverUtils
    {
        public static float WeightSum(this List<ResolvedPattern> resolvedPatterns)
        {
            float sum = 0;

            foreach (ResolvedPattern pattern in resolvedPatterns)
            {
                sum += pattern.weight;
            }

            return sum;
        }

        public static ResolvedPattern RandomWeightedSelection(this List<ResolvedPattern> candidates)
        {
            float selected = Random.Range(0f, candidates.WeightSum());
            Debug.Log(selected);
            foreach (ResolvedPattern pattern in candidates)
            {
                selected -= pattern.weight;

                if (selected < 0)
                {
                    return pattern;
                }
            }

            return candidates[^1];
        }
    }
}