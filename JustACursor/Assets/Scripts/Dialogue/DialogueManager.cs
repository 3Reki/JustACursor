using System.Collections;
using System.Collections.Generic;
using Graph;
using Graph.Dialogue;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Dialogue.New
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private GameObject responseParent;
        [SerializeField] private TMP_Text textLabel;
        [SerializeField] private WriterEffect writerEffect;
        [SerializeField] private DialogueOption[] options;
        
        [Header("DEBUG")]
        [SerializeField] private DialogueGraph testDialogue;
        
        private PlayerInputs inputs;
        private bool interactInput;
        
        private DialogueGraph currentGraph;
        private Coroutine dialogueCoroutine;

        private void Start()
        {
            inputs = InputManager.Instance.inputs;
            Play(testDialogue);
        }

        private void Update()
        {
            interactInput = inputs.Dialogue.Interact.WasPressedThisFrame();
        }
        
        public void Play(DialogueGraph graph)
        {
            currentGraph = graph;
            currentGraph.Start();

            dialogueBox.SetActive(true);
            dialogueCoroutine = StartCoroutine(DialogueCR());
        }
        
        public void Stop()
        {
            StopCoroutine(dialogueCoroutine);
            
            currentGraph = null;
            dialogueBox.SetActive(false);
        }

        private IEnumerator DialogueCR()
        {
            DialogueNode node = currentGraph.currentNode as DialogueNode;
            writerEffect.Run(node.Dialogue, textLabel);

            //Prevent skipping
            yield return null;
            
            while (writerEffect.IsWriting)
            {
                if (inputs.Dialogue.Interact.WasPressedThisFrame())
                    writerEffect.Complete();
                
                yield return null;
            }

            if (node.Responses.Length > 0)
            {
                ShowResponses(node);
                yield break;
            }

            while (!interactInput) yield return null;

            GoToNextNode("Default");
        }

        private void GoToNextNode(string nextNode, DialogueEvent eventToTrigger = DialogueEvent.None)
        {
            TriggerEvent(eventToTrigger);
            responseParent.SetActive(false);

            currentGraph.currentNode = currentGraph.currentNode.NextNode(nextNode);
            if (currentGraph.currentNode.GetType() == typeof(DialogueNode)) StartCoroutine(DialogueCR());
            else if (currentGraph.currentNode.GetType() == typeof(StopNode)) Stop();
            else Debug.LogError("Invalid Node type");
        }

        private void ShowResponses(DialogueNode node)
        {
            responseParent.SetActive(true);
            for (int i = 0; i < options.Length; i++)
            {
                DialogueOption option = options[i];
                if (i < node.Responses.Length)
                {
                    int index = i;
                    Response response = node.Responses[index];
                    
                    option.gameObject.SetActive(true);
                    option.SetAction(() =>
                    {
                        GoToNextNode($"Responses {index}", response.Event);
                    });
                    option.SetText(response.Text);
                }
                else option.gameObject.SetActive(false);
            }
        }

        private void TriggerEvent(DialogueEvent responseEvent)
        {
            // TODO: Some functions called when specific choices are made
            Debug.Log(responseEvent);
        }
    }
}