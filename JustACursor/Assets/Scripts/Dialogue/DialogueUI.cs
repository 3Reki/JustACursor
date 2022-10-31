using System;
using System.Collections;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Dialogue {
    public class DialogueUI : MonoBehaviour {

        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private TMP_Text textLabel;
        [SerializeField] private WriterEffect writerEffect;
        [SerializeField] private DialogueObject testDialogue;
        
        private PlayerInputs inputs;

        private void Start() {
            inputs = new PlayerInputs();
            inputs.Enable();
            
            ShowDialogue(testDialogue);
        }

        private void ShowDialogue(DialogueObject dialogueObject) {
            dialogueBox.SetActive(true);
            StartCoroutine(StepThroughDialogue(dialogueObject));
        }

        private IEnumerator StepThroughDialogue(DialogueObject dialogueObject) {
            foreach (string dialogue in dialogueObject.Dialogue) {
                yield return writerEffect.Run(dialogue, textLabel);
                yield return new WaitUntil(inputs.Dialogue.Interact.WasPressedThisFrame);
            }
            
            Close();
        }

        private void Close() {
            dialogueBox.SetActive(false);
            textLabel.text = String.Empty;
        }
    }
}

