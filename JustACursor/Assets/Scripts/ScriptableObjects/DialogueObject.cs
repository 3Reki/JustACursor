using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(menuName = "Just A Cursor/DialogueObject", fileName = "New Dialogue")]
    public class DialogueObject : ScriptableObject {
        
        [SerializeField, TextArea] private string[] dialogue;
        public string[] Dialogue => dialogue;
    }
}

