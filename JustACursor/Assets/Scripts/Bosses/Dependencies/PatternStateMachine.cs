﻿using System;
using System.Collections.Generic;
using Bosses.Patterns;
using Graph;
using Graph.Resolver;
using UnityEngine;

namespace Bosses.Dependencies
{
    [CreateAssetMenu(fileName = "StateMachine", menuName = "Just A Cursor/State Machine", order = 0)]
    public class PatternStateMachine : ScriptableObject
    {
        [SerializeField] private ResolverGraph resolverGraph;

        private Boss target;
        private Pattern<Boss> currentInstruction;
        private readonly List<ResolvedPattern> selectedList = new();
        private bool hasEnded;
        private int nodeIndex;

        public void Play(Boss boss)
        {
            target = boss;
            resolverGraph.Start();
            hasEnded = false;
            nodeIndex = 1;
        }

        public void UpdateMachine()
        {
            if (hasEnded)
            {
                Play(target);
                return;
            }
            
            // TODO
            // if (currentInstruction == null)
            // {
            //     currentInstruction = Resolve(target);
            //     return;
            // }
            //
            // switch (currentInstruction.phase)
            // {
            //     case InstructionPhase.None:
            //         currentInstruction = Resolve(target);
            //         break;
            //     case InstructionPhase.Start:
            //         currentInstruction.Play(target);
            //         break;
            //     case InstructionPhase.Update:
            //         currentInstruction.Update();
            //         break;
            //     case InstructionPhase.Stop:
            //         currentInstruction = currentInstruction.Stop();
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException();
            // }
        }
        
        public void Stop()
        {
            if (currentInstruction != null)
            {
                currentInstruction.Stop();
            }
        }

        // public Instruction<Boss> Resolve(Boss boss)
        // {
        //     selectedList.Clear();
        //     
        //     foreach (ResolvedPattern choice in GetChoices())
        //     {
        //         if (choice.condition.Check(boss)) selectedList.Add(choice); 
        //     }
        //
        //     if (selectedList.Count == 0) return null;
        //
        //     Instruction<Boss> instruction = selectedList.RandomWeightedSelection().pattern;
        //     instruction.phase = InstructionPhase.Start;
        //     
        //     for (int i = 0; i < selectedList.Count; i++)
        //     {
        //         if (instruction != selectedList[i].pattern) continue;
        //         
        //         Debug.Log($"{nodeIndex}) {instruction.name}");
        //         GoToNextNode($"choices {i}");
        //         nodeIndex++;
        //         break;
        //     }
        //     return instruction;
        // }
        
        private List<ResolvedPattern> GetChoices() {
            ResolverNode node = (ResolverNode) resolverGraph.currentNode;
            return node.Choices;
        }
        
        private void GoToNextNode(string nextNode)
        {
            resolverGraph.currentNode = resolverGraph.currentNode.NextNode(nextNode);
            
            if (resolverGraph.currentNode.GetType() != typeof(StopNode)) return;
            
            Debug.Log("End of Resolver");
            hasEnded = true;
        }
    }
}