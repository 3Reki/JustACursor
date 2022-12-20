using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Dialogue
{
    public class DialogueOption : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text textLabel;

        public void SetAction(UnityAction action)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }

        public void SetText(string text)
        {
            textLabel.text = text;
        }
    }
}