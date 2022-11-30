using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class GameMenuHandler : MonoBehaviour
    {
        [SerializeField] private GameObject startMenu;
        
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
            if (inputs.UI.Pause.WasPressedThisFrame()) {
                if (uiStack.Count == 0) Open(startMenu);
                else if (uiStack.Count == 1) Close();
                
                return;
            }
            
            if (inputs.UI.Back.WasPressedThisFrame())
            {
                if (uiStack.Count <= 1) return;

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
            
            if (uiStack.Count == 0) Time.timeScale = 0;
            else if (uiStack.Count > 0) uiStack.Peek().SetActive(false);
            
            go.SetActive(true);
            uiStack.Push(go);
        }

        public void Close()
        {
            if (uiStack.Count == 0) return;
            
            uiStack.Pop().SetActive(false);
            if (uiStack.Count > 0)
            {
                uiStack.Peek().SetActive(true);
            }

            if (uiStack.Count == 0) Time.timeScale = 1;
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