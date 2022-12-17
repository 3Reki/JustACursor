namespace Dialogue.New
{
    [NodeTint("#228B22")]
    [NodeWidth(150)]
    public class StartNode : BaseNode
    {
        [Output(connectionType = ConnectionType.Override)] public int exit;
    }
}