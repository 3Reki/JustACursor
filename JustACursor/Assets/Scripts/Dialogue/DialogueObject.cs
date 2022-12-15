using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(menuName = "Just A Cursor/Dialogue", fileName = "New Dialogue")]
    public class DialogueObject : ScriptableObject
    {
        [SerializeField, TextArea] private string[] dialogue;
        [SerializeField] private Response[] responses;
        
        public string[] Dialogue => dialogue;
        public Response[] Responses => responses;
        public bool HasResponses => responses is { Length: > 1 };
    }
}
