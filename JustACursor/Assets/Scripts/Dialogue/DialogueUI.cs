using TMPro;
using UnityEngine;

namespace Dialogue {
    public class DialogueUI : MonoBehaviour {
    
        [SerializeField] private TMP_Text textLabel;
        [SerializeField] private WriterEffect writerEffect;

        private void Start() {
            writerEffect.Run("This is a bit of text\nHello !", textLabel);
        }
    }
}

