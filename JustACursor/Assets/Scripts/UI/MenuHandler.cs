using Managers;
using UnityEngine;

namespace UI
{
    public class MenuHandler : MonoBehaviour
    {
        private PlayerInputs inputs;

        private void Start()
        {
            inputs = InputManager.Instance.inputs;
        }
        
        private void Update()
        {
            if (inputs.Menu.Pause.WasPressedThisFrame())
            {
                onPauseButtonPressed.Invoke();
            }

            else if (inputs.Menu.Return.WasPressedThisFrame())
            {
                onBackButtonPressed.Invoke();
            }
        }
        
        public delegate void OnPauseButtonPressed();
        public static OnPauseButtonPressed onPauseButtonPressed;
        
        public delegate void OnBackButtonPressed();
        public static OnBackButtonPressed onBackButtonPressed;
    }
}