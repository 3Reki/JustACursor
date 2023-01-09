using UnityEngine;
using XNode;

namespace Graph {
    public class BaseGraph : NodeGraph
    {
        private BaseNode startNode;
        [HideInInspector] public BaseNode CurrentNode;

        public void Start()
        {
            if (startNode == null) Init();
            
            CurrentNode = startNode.NextNode("exit");
        }

        private void Init() {
            foreach (Node node in nodes) {
                if (node.name == "Start") {
                    startNode = node as BaseNode;
                    break;
                }
            }
        }
    }
}