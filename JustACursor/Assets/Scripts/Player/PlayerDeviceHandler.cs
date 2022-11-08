using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerDeviceHandler : MonoBehaviour
    {
        private List<SchemeAim> schemeList = new()
        {
            new("Keyboard & Mouse", AimMethod.Mouse),
            new("Gamepad", AimMethod.Gamepad)
        };
            
        public AimMethod currentAimMethod { get; private set; }
    
        [SerializeField] private PlayerInput playerInput;
    
        private string currentScheme;
        
        private void Start()
        {
            currentScheme = playerInput.currentControlScheme;
            currentAimMethod = GetAimMethod(currentScheme);
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
            currentAimMethod = GetAimMethod(currentScheme);
    
            Cursor.visible = currentAimMethod == AimMethod.Mouse;
        }
    
        private AimMethod GetAimMethod(string schemeName)
        {
            foreach (SchemeAim item in schemeList)
            {
                if (item.schemeName.Equals(schemeName)) return item.aimMethod;
            }
    
            return AimMethod.Mouse;
        }
    
        private class SchemeAim
        {
            public string schemeName;
            public AimMethod aimMethod;
    
            public SchemeAim(string schemeName, AimMethod aimMethod)
            {
                this.schemeName = schemeName;
                this.aimMethod = aimMethod;
            }
        }
    
        public enum AimMethod
        {
            Mouse,
            Gamepad
        }
    }
}

