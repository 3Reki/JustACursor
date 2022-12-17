using System;
using UnityEngine;

namespace Dialogue.Old
{
    [Serializable]
    public class Response
    {
        [field: SerializeField, TextArea(2,2)] public string ResponseText { get; private set; }
        [field: SerializeField] public DialogueEvent ResponseEvent { get; private set; }
        [field: SerializeField] public DialogueObject NextDialogue { get; private set; }
    }
    
    public enum DialogueEvent
    {
        None = 0, A, B
    }
}


