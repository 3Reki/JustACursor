using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace UI
{
    public class MenuHandler : MonoBehaviour
    {
        private PlayerInputs inputs;
        private Stack<GameObject> uiStack = new();

        [SerializeField] private GameObject startMenu;

        private void Start()
        {
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
                if (uiStack.Count > 1) Close();
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
    }
}