using System;
using ScriptableObjects;
using UnityEngine;

namespace Dialogue
{
    [Serializable]
    public class Response
    {
        [field: SerializeField] public DialogueEvent ResponseEvent { get; private set; }
        [field: SerializeField] public string ResponseText { get; private set; }
        [field: SerializeField] public DialogueObject NextDialogue { get; private set; }
    }
    
    public enum DialogueEvent
    {
        None = 0, A, B
    }
}


