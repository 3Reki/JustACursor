namespace Managers
{
    public class InputManager : Singleton<InputManager>
    {
        public PlayerInputs inputs;

        private void Awake()
        {
            inputs = new PlayerInputs();
            inputs.Enable();
        }
    }
}

