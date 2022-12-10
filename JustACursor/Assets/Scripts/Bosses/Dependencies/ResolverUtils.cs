using System.Collections.Generic;
using UnityEngine;

namespace Bosses.Dependencies
{
    public static class ResolverUtils
    {
        public static float WeightSum<T>(this List<ResolvedPattern<T>> resolvedPatterns) where T : Bosses.Boss
        {
            float sum = 0;

            foreach (ResolvedPattern<T> pattern in resolvedPatterns)
            {
                sum += pattern.weight;
#if UNITY_EDITOR
                if (pattern.weight < 0.001f)
                {
                    Debug.LogWarning($"{pattern.pattern.name} has a weight of 0 !");
                }
#endif
            }

            return sum;
        }

        public static ResolvedPattern<T> RandomWeightedSelection<T>(this List<ResolvedPattern<T>> candidates) where T : Bosses.Boss
        {
            float selected = Random.Range(0f, candidates.WeightSum());
            Debug.Log(selected);
            foreach (ResolvedPattern<T> pattern in candidates)
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