using System.Collections;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Dialogue
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private GameObject responses;
        [SerializeField] private TMP_Text textLabel;
        [SerializeField] private WriterEffect writerEffect;
        
        [Header("DEBUG")]
        [SerializeField] private DialogueObject testDialogue;

        private PlayerInputs inputs;
        private bool interactInput;

        private void Start()
        {
            inputs = new PlayerInputs();
            inputs.Enable();
            
            ShowDialogue(testDialogue);
        }

        private void Update()
        {
            interactInput = inputs.Dialogue.Interact.WasPressedThisFrame();
        }

        private void ShowDialogue(DialogueObject dialogueObject)
        {
            dialogueBox.SetActive(true);
            StartCoroutine(StepThroughDialogue(dialogueObject));
        }

        private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
        {
            textLabel.text = string.Empty;
            int index = 0;

            foreach (string dialogue in dialogueObject.Dialogue)
            {
                index++;
                
                writerEffect.Run(dialogue, textLabel);
                while (!interactInput) yield return null;
                interactInput = false;

                //If text not fully displayed, display it entirely
                if (writerEffect.IsWriting)
                {
                    writerEffect.Complete(dialogue, textLabel);
                }

                //If last dialog and has responses
                if (index == dialogueObject.Dialogue.Length && dialogueObject.HasResponses)
                {
                    ShowResponses();
                    yield break;
                }
                
                while (!interactInput) yield return null;
                interactInput = false;
            }
            
            Close();
        }

        private void ShowResponses()
        {
            responses.SetActive(true);
        }

        private void Close()
        {
            dialogueBox.SetActive(false);
            responses.SetActive(false);
        }

        public void TriggerEvent(DialogueObject.DialogueEvent responseEvent)
        {
            // TODO: Some functions called when specific choices are made
            Close();
        }
    }
}

