using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_NAME", menuName = "Just A Cursor/Pattern/Pattern Queue", order = 0)]
    public class PatternQueue : Pattern<Boss>
    {
        [SerializeField] private Pattern<Boss>[] instructions;

        private readonly Queue<Pattern<Boss>> patternQueue = new();
        private Pattern<Boss> currentPattern;

        public override void Play(Boss boss)
        {
            base.Play(boss);
            
            patternQueue.Clear();
            currentPattern = null;
            foreach (Pattern<Boss> instruction in instructions)
            {
                patternQueue.Enqueue(instruction);
            }
        }

        public override void Update()
        {
            if (currentPattern == null)
            {
                currentPattern = patternQueue.Dequeue();
                currentPattern.phase = PatternPhase.Start;
            }
            
            switch (currentPattern.phase)
            {
                case PatternPhase.None:
                    currentPattern = patternQueue.Dequeue();
                    currentPattern.phase = PatternPhase.Start;
                    break;
                case PatternPhase.Start:
                    currentPattern.Play(linkedBoss);
                    break;
                case PatternPhase.Update:
                    currentPattern.Update();
                    break;
                case PatternPhase.Stop:
                    currentPattern.Stop();
                    
                    if (patternQueue.Count == 0)
                    {
                        phase = PatternPhase.Stop;
                    }
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override Pattern<Boss> Stop()
        {
            if (currentPattern.phase is PatternPhase.Update or PatternPhase.Stop)
            {
                currentPattern.Stop();
            }
            
            return base.Stop();
        }
    }
}