using System.Collections.Generic;
using UnityEngine;

namespace Bosses.Dependencies
{
    public static class ResolverUtils
    {
        public static float WeightSum(this List<ResolvedPattern> resolvedPatterns)
        {
            float sum = 0;

            foreach (ResolvedPattern pattern in resolvedPatterns)
            {
                sum += pattern.weight;
#if UNITY_EDITOR
                if (pattern.weight < 0.001f)
                {
                    Debug.LogWarning("A pattern has a weight of 0 !");
                }
#endif
            }

            return sum;
        }

        public static int RandomWeightedSelection(this List<ResolvedPattern> candidates)
        {
            float selected = Random.Range(0f, candidates.WeightSum());

            for (var i = 0; i < candidates.Count; i++)
            {
                selected -= candidates[i].weight;

                if (selected < 0)
                {
                    return i;
                }
            }

            return candidates.Count - 1;
        }
    }
}