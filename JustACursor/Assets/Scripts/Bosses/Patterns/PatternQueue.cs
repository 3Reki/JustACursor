using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bosses.Patterns
{
    public class PatternQueue<T> : Pattern<T> where T : Boss
    {
        [SerializeField] private Pattern<T>[] instructions;

        private readonly Queue<Pattern<T>> patternQueue = new();
        private Pattern<T> currentPattern;

        public override void Play(T boss)
        {
            base.Play(boss);
            
            patternQueue.Clear();
            currentPattern = null;
            foreach (Pattern<T> instruction in instructions)
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

        public override Pattern<T> Stop()
        {
            if (currentPattern.phase is PatternPhase.Update or PatternPhase.Stop)
            {
                currentPattern.Stop();
            }
            
            return base.Stop();
        }
    }
}