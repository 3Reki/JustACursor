using System;
using Bosses.Patterns;
using Graph;
using Graph.Resolver;
using UnityEngine;

namespace Bosses.Dependencies
{
    public class PatternStateMachine<T> : ScriptableObject where T : Boss
    {
        [SerializeField] private ResolverGraph resolverGraph;

        private T target;
        private Pattern<T> currentPattern;
        private bool hasEnded;

        public void Play(T boss)
        {
            target = boss;
            resolverGraph.Start();
            hasEnded = false;
        }

        public void UpdateMachine()
        {
            if (hasEnded)
            {
                Play(target);
                return;
            }

            if (resolverGraph.currentNode.GetType() == typeof(ResolverNode))
            {
                int choiceIndex = ((ResolverNode) resolverGraph.currentNode).Resolve(target);
                GoToNextNode($"choices {choiceIndex}");
                return;
            }

            var pattern = resolverGraph.currentNode as Pattern<T>;
            if (pattern)
            {
                switch (pattern.currentState)
                {
                    case Pattern<T>.State.Start:
                        pattern.Play(target);
                        currentPattern = pattern;
                        break;
                    case Pattern<T>.State.Update:
                        pattern.Update();
                        break;
                    case Pattern<T>.State.Stop:
                        pattern.Stop();
                        currentPattern = null;
                        GoToNextNode("exit");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return;
            }
            
            Debug.Log("Not pattern or resolver.");
        }
        
        public void Stop()
        {
            if (currentPattern != null)
            {
                currentPattern.Stop();
            }
        }
        
        private void GoToNextNode(string nextNode)
        {
            resolverGraph.currentNode = resolverGraph.currentNode.NextNode(nextNode);
            
            if (resolverGraph.currentNode.GetType() == typeof(StopNode))
                hasEnded = true;
            else
                UpdateMachine();
        }
    }
}