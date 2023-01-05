using System;
using System.Collections.Generic;
using System.Linq;
using Graph.Resolver;
using LegacyBosses;
using LegacyBosses.Dependencies;
using LegacyBosses.Instructions;
using UnityEngine;

namespace Graph
{
    [Serializable]
    public class ResolverTwo : MonoBehaviour {
        [SerializeField] private ResolverGraph testResolver;
        
        private readonly List<ResolvedPattern<Boss>> selectedList = new();
        private Boss rdmBoss;

        private bool hasEnded;
        
        private void Update() {
            if (Input.GetKeyDown(KeyCode.R)) {
                testResolver.Start();
                hasEnded = false;
                int node = 1;
                Debug.Log("Start of Resolver");
                
                while (!hasEnded) {
                    Instruction<Boss> instruction = Resolve(rdmBoss);
                    for (int i = 0; i < selectedList.Count; i++) {
                        if (instruction == selectedList[i].pattern) {
                            Debug.Log($"{node}) {instruction.name}");
                            GoToNextNode($"Choices {i}");
                            node++;
                            break;
                        }
                    }
                }
            }
        }
        
        private Instruction<Boss> Resolve(Boss boss)
        {
            selectedList.Clear();
            
            foreach (ResolvedPattern<Boss> choice in GetChoices())
            {
                if (choice.condition.Check(boss)) selectedList.Add(choice); 
            }

            if (selectedList.Count == 0) return null;

            Instruction<Boss> instruction = selectedList.RandomWeightedSelection().pattern;
            instruction.phase = InstructionPhase.Start;
            return instruction;
        }

        private List<ResolvedPattern<Boss>> GetChoices() {
            ResolverNode node = testResolver.currentNode as ResolverNode;
            return node.Choices;
        }
        
        private void GoToNextNode(string nextNode)
        {
            testResolver.currentNode = testResolver.currentNode.NextNode(nextNode);
            if (testResolver.currentNode.GetType() == typeof(StopNode)) {
                Debug.Log("End of Resolver");
                hasEnded = true;
            }
        }
    }
}