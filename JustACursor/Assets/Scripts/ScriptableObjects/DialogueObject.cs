using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Just A Cursor/Dialogue", fileName = "New Dialogue")]
    public class DialogueObject : ScriptableObject
    {
        [SerializeField, TextArea] private string[] dialogue;
        [SerializeField] private Response[] responses;
        
        public string[] Dialogue => dialogue;
        public Response[] Responses => responses;
        public bool HasResponses => responses is { Length: > 1 };

        [Serializable]
        public class Response
        {
            [SerializeField] private DialogueEvent responseEvent;
            [SerializeField] private string responseText;
            
            public DialogueEvent Event => responseEvent;
            public string Text => responseText;
        }

        public enum DialogueEvent
        {
            None = 0, A, B
        }
    }
}
