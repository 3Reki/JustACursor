using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Managers
{
    public class InputManager : Singleton<InputManager>
    {
        public PlayerInputs inputs; //InputActions
        [SerializeField] private PlayerInput playerInput; //Component

        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private InputActionAsset defaultInputAction;
        [SerializeField] private InputActionAsset playerInputAction;
        
        private string currentScheme;

        public override void Awake()
        {
            base.Awake();
            
            inputs = new PlayerInputs();
            inputs.Enable();
        }

        /*private void Start()
        {
            currentScheme = playerInput.currentControlScheme;
        }

        private void OnEnable()
        {
            playerInput.onControlsChanged += OnControlsChanged;
        }

        private void OnDisable()
        {
            playerInput.onControlsChanged -= OnControlsChanged;
        }

        private void OnControlsChanged(PlayerInput input)
        {
            currentScheme = playerInput.currentControlScheme;
            Debug.Log(currentScheme);
            eventSystem.GetComponent<InputSystemUIInputModule>().actionsAsset = currentScheme.Equals("Gamepad") ? playerInputAction : defaultInputAction;
        }*/
    }
}

