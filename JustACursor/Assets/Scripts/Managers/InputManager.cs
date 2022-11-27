namespace Managers
{
    public class InputManager : Singleton<InputManager>
    {
        public PlayerInputs inputs;
        
        private string currentScheme;

        public override void Awake()
        {
            base.Awake();
            
            inputs = new PlayerInputs();
            inputs.Enable();
        }
    }
}

