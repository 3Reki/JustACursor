using Dialogue.Old;
using UnityEngine;
using XNode;

namespace Dialogue.New
{
    [NodeWidth(500)]
    public class DialogueNode : BaseNode
    {
        [Input(ShowBackingValue.Always), TextArea(5,5)] public string Dialogue;
        [Output(dynamicPortList = true)] public Response[] Responses;
        [Output] public string Default;
    }
}