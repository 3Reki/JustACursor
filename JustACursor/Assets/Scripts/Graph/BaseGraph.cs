using XNode;

namespace Graph {
    public class BaseGraph : NodeGraph {
        public BaseNode startNode;
        public BaseNode currentNode;

        public void Start()
        {
            if (startNode == null) Init();
            
            currentNode = startNode.NextNode("exit");
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