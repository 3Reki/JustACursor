using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace Dialogue
{
    public class WriterEffect : MonoBehaviour
    {
        public bool IsWriting { get; private set; }

        [SerializeField] private float writingSpeed;

        private Coroutine typeCR;
        private string textToType;
        private TMP_Text textLabel;

        public void Run(string textToType, TMP_Text textLabel)
        {
            this.textToType = textToType;
            this.textLabel = textLabel;
            typeCR = StartCoroutine(TypeText());
        }

        private IEnumerator TypeText() {
            textLabel.text = string.Empty;
            IsWriting = true;
            
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
            IsWriting = false;
        }

        public void Complete()
        {
            StopCoroutine(typeCR);
            
            textLabel.text = textToType;

            textToType = null;
            textLabel = null;
            IsWriting = false;
        }
    }
}

