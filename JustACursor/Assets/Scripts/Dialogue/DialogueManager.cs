using System.Collections;
using Graph;
using Graph.Dialogue;
using Player;
using TMPro;
using UnityEngine;
using Utils;

namespace Dialogue
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        [Header("UI")]
        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private GameObject responseParent;
        [SerializeField] private TMP_Text textLabel;
        [SerializeField] private DialogueOption[] options;
        [SerializeField] private WriterEffect writerEffect;
        
        [Header("Dialogue")]
        [SerializeField] private DialogueGraph currentDialogue;
        
        private PlayerInputs inputs;
        private bool interactInput;
        
        private DialogueGraph currentGraph;
        private Coroutine dialogueCoroutine;
        
        public delegate void NextDialogue();
        public static NextDialogue nextDialogue;

        private void Start()
        {
            inputs = InputManager.Instance.inputs;
            Play(currentDialogue);
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
        
        private void Stop()
        {
            StopCoroutine(dialogueCoroutine);
            
            currentGraph = null;
            dialogueBox.SetActive(false);
        }

        private IEnumerator DialogueCR()
        {
            DialogueNode node = currentGraph.CurrentNode as DialogueNode;
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

            currentGraph.CurrentNode = currentGraph.CurrentNode.NextNode(nextNode);
            if (currentGraph.CurrentNode.GetType() == typeof(DialogueNode)) StartCoroutine(DialogueCR());
            else if (currentGraph.CurrentNode.GetType() == typeof(StopNode)) Stop();
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