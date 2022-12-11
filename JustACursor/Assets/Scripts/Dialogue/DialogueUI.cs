using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

namespace Dialogue
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private GameObject responseParent;
        [SerializeField] private List<Button> responseButtons;
        [SerializeField] private TMP_Text textLabel;
        [SerializeField] private WriterEffect writerEffect;
        
        [Header("DEBUG")]
        [SerializeField] private DialogueObject testDialogue;

        private PlayerInputs inputs;
        private bool interactInput;

        private void Start()
        {
            inputs = InputManager.Instance.inputs;
            
            ShowDialogue(testDialogue);
        }

        private void Update()
        {
            interactInput = inputs.Dialogue.Interact.WasPressedThisFrame();
        }

        private void ShowDialogue(DialogueObject dialogueObject)
        {
            dialogueBox.SetActive(true);
            responseParent.SetActive(false);
            StartCoroutine(StepThroughDialogue(dialogueObject));
        }
        
        private void ShowResponses(Response[] responses)
        {
            if (responses.Length > responseButtons.Count)
            {
                Debug.LogError("Too much responses provided");
            }
            
            responseParent.SetActive(true);
            for (int i = 0; i < responseButtons.Count; i++)
            {
                responseButtons[i].onClick.RemoveAllListeners();
                
                int index = i;
                responseButtons[i].onClick.AddListener(() =>
                {
                    TriggerEvent(responses[index].ResponseEvent);
                    DialogueObject nextDialogue = responses[index].NextDialogue;
                    if (nextDialogue != null)
                    {
                        ShowDialogue(nextDialogue);
                    }
                });
            }
        }

        private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
        {
            textLabel.text = string.Empty;
            int index = 0;

            foreach (string dialogue in dialogueObject.Dialogue)
            {
                index++;

                writerEffect.Run(dialogue, textLabel);
                while (writerEffect.IsWriting)
                {
                    if (interactInput)
                    {
                        interactInput = false;
                        writerEffect.Complete(dialogue, textLabel);
                    }
                    yield return null;
                }
                
                //If last dialog and has responses
                if (index == dialogueObject.Dialogue.Length && dialogueObject.HasResponses)
                {
                    ShowResponses(dialogueObject.Responses);
                    yield break;
                }

                while (!interactInput) yield return null;
                interactInput = false;
            }
            
            Close();
        }

        private void TriggerEvent(DialogueEvent responseEvent)
        {
            // TODO: Some functions called when specific choices are made
            Debug.Log(responseEvent);
            Close();
        }
        
        private void Close()
        {
            dialogueBox.SetActive(false);
            responseParent.SetActive(false);
        }
    }
}

