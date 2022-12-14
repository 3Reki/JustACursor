using System;
using System.Collections.Generic;
using Bosses.Patterns;
using UnityEngine;

namespace Bosses.Instructions
{
    public class InstructionSequence<T> : Instruction<T> where T : Boss
    {
        [SerializeField] private Pattern<T>[] instructions;

        private readonly Queue<Pattern<T>> patternQueue = new();
        private Pattern<T> currentPattern;

        public override void Play(T entity)
        {
            base.Play(entity);
            
            patternQueue.Clear();
            foreach (Pattern<T> instruction in instructions)
            {
                patternQueue.Enqueue(instruction);
            }

            currentPattern = patternQueue.Dequeue();
            currentPattern.Play(linkedEntity);
        }

        public override void Update()
        {
            if (!currentPattern.isFinished)
            {
                currentPattern.Update();
                return;
            }
            
            if (patternQueue.Count == 0)
            {
                phase = InstructionPhase.Stop;
                return;
            }
                
            currentPattern.Stop();
            currentPattern = patternQueue.Dequeue();
            currentPattern.Play(linkedEntity);
        }

        public override Instruction<T> Stop()
        {
            currentPattern.Stop();

            return base.Stop();
        }
    }
}