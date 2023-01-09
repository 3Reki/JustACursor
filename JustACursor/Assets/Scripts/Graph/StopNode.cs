using Graph.Dialogue;

namespace Graph
{
    [NodeTint("#8B0000")]
    [NodeWidth(150)]
    public class StopNode : BaseNode
    {
        [Input(ShowBackingValue.Never)] public int entry;
    }
}