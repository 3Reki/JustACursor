using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ScrollRectAutoScroll : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;

        private EventSystem eventSystem;

        private void Start()
        {
            eventSystem = EventSystem.current;
        }
        
        private void Update()
        {
            int childCount = scrollRect.content.childCount-1;
            int childIndex = eventSystem.currentSelectedGameObject.transform.GetSiblingIndex()-1;
            childIndex = childIndex > (float)childCount / 2 ? childIndex + 1 : childIndex;
            scrollRect.verticalScrollbar.value = 1 - (float)childIndex / childCount;
        }
    }
}