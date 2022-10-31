using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace Dialogue {
    public class WriterEffect : MonoBehaviour {

        [SerializeField] private float writingSpeed;
        
        public void Run(string textToType, TMP_Text textLabel) {
            StartCoroutine(TypeText(textToType,textLabel));
        }

        private IEnumerator TypeText(string textToType, TMP_Text textLabel) {
            yield return new WaitForSeconds(2);
            
            textLabel.text = string.Empty;
            
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
        }
    }
}

