using System;
using UnityEngine;

namespace Dialogue
{
    [Serializable]
    public class Response
    {
        [field: SerializeField, TextArea(2,2)] public string Text { get; private set; }
        [field: SerializeField, NodeEnum] public DialogueEvent Event { get; private set; }
    }
    
    public enum DialogueEvent
    {
        None = 0, A, B
    }
}


