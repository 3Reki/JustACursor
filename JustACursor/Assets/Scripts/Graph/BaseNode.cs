using XNode;

namespace Graph
{
    public class BaseNode : Node
    {
        //Prevent warnings in console
        public override object GetValue(NodePort port) {
            return null;
        }

        public BaseNode NextNode(string _exit)
        {
            foreach (NodePort port in Ports)
            {
                if (port.fieldName != _exit) continue;
                return port.Connection.node as BaseNode;
            }

            return null;
        }
    }
}