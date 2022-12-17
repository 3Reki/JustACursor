namespace Dialogue.New
{
    [NodeTint("#8B0000")]
    [NodeWidth(150)]
    public class StopNode : BaseNode
    {
        [Input(ShowBackingValue.Never)] public int entry;
    }
}