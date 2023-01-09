using Dialogue;
using UnityEngine;

namespace Graph.Dialogue
{
    [NodeWidth(300)]
    public class DialogueNode : BaseNode
    {
        [Input(ShowBackingValue.Always), TextArea(5,5)] public string Dialogue;
        [Output(dynamicPortList = true, connectionType = ConnectionType.Override)] public Response[] Responses;
        [Output(connectionType = ConnectionType.Override)] public string Default;
    }
}