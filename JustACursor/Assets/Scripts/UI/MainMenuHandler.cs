using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class MainMenuHandler : MonoBehaviour
    {
        [SerializeField] private GameObject main;
        
        private PlayerInputs inputs;
        private Stack<GameObject> uiStack = new();
        
        private GameObject currentConfirmation;
        private GameObject lastSelectedElement;

        private EventSystem eventSystem;

        private void Start()
        {
            eventSystem = EventSystem.current;
            inputs = InputManager.Instance.inputs;
        }
        
        private void Update()
        {
            if (inputs.UI.Back.WasPressedThisFrame())
            {
                TMP_Dropdown checkDropdown = eventSystem.currentSelectedGameObject.GetComponent<TMP_Dropdown>();
                if (checkDropdown && checkDropdown.IsExpanded)
                {
                    checkDropdown.Hide();
                    return;
                }

                if (currentConfirmation != null)
                {
                    CloseConfirmation();
                    return;
                }
                
                Close();
            }
        }

        public void Open(GameObject go)
        {
            if (uiStack.Contains(go))
            {
                Debug.LogError("Object already in stack");
                return;
            }
            
            if (uiStack.Count > 0) uiStack.Peek().SetActive(false);
            
            go.SetActive(true);
            uiStack.Push(go);
            
            main.SetActive(false);
        }

        public void Close()
        {
            if (uiStack.Count == 0) return;
            
            uiStack.Pop().SetActive(false);
            if (uiStack.Count > 0)
            {
                uiStack.Peek().SetActive(true);
            }
            
            if (uiStack.Count == 0) main.SetActive(true);
        }

        public void OpenConfirmation(GameObject go)
        {
            lastSelectedElement = eventSystem.currentSelectedGameObject;
            
            currentConfirmation = go;
            currentConfirmation.SetActive(true);
        }

        public void CloseConfirmation()
        {
            currentConfirmation.SetActive(false);
            currentConfirmation = null;

            eventSystem.SetSelectedGameObject(lastSelectedElement);
        }
    }
}
