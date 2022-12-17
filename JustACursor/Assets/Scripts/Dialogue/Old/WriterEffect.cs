using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace Dialogue {
    public class WriterEffect : MonoBehaviour
    {
        public bool IsWriting => isWriting;
        
        [SerializeField] private float writingSpeed;

        private Coroutine typeCR;
        private bool isWriting;
        
        public void Run(string textToType, TMP_Text textLabel) {
            typeCR = StartCoroutine(TypeText(textToType,textLabel));
        }

        private IEnumerator TypeText(string textToType, TMP_Text textLabel) {
            textLabel.text = string.Empty;
            isWriting = true;
            
            float time = 0;
            int charIndex = 0;

            while (charIndex < textToType.Length) {
                time += Time.deltaTime*writingSpeed;
                charIndex = Mathf.FloorToInt(time);
                charIndex = Math.Clamp(charIndex, 0, textToType.Length);

                textLabel.text = textToType.Substring(0, charIndex);

                yield return null;
            }

            textLabel.text = textToType;
            isWriting = false;
        }

        public void Complete(string textToType, TMP_Text textLabel)
        {
            StopCoroutine(typeCR);
            
            textLabel.text = textToType;
            isWriting = false;
        }
    }
}

