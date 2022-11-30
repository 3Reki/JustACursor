using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
    public class AutoSelect : MonoBehaviour, IPointerEnterHandler {

        [SerializeField] private bool isFirstElement;
        
        private Selectable selectable;

        private void OnEnable() {
            selectable = GetComponent<Selectable>();
            if (isFirstElement) selectable.Select();
        }

        public void OnPointerEnter(PointerEventData eventData) {
            selectable.Select();
        }
    }
}