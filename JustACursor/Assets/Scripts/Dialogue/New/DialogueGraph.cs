using UnityEngine;
using XNode;

namespace Dialogue.New
{
    [CreateAssetMenu(menuName = "Just A Cursor/Graph/Dialogue", fileName = "New Dialogue")]
    public class DialogueGraph : NodeGraph
    {
        public BaseNode startNode;
        public BaseNode currentNode;

        public void Start()
        {
            currentNode = startNode.NextNode("exit");
        }
    }
}