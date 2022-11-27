using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class AutoSelectOnEnable : MonoBehaviour {

        private Selectable selectable;
        
        private void OnEnable() {
            if (selectable == null) Init();
            selectable.Select();
        }

        private void OnDisable() {
            selectable.Select();
        }

        private void Init() {
            selectable = GetComponent<Selectable>();
        }
    }
}