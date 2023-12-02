using NaughtyAttributes;
using System.Collections;
using UnityEngine;

namespace Bipolar.UI
{
    [RequireComponent (typeof(TextChangeDetector))]
    public class LetterByLetterTextEffect : MonoBehaviour
    {
        private TextChangeDetector textChangeDetector;
        public TextChangeDetector Detector
        {
            get
            {
                if (textChangeDetector == null)
                    textChangeDetector = GetComponent<TextChangeDetector>();
                return textChangeDetector;
            }
        }

        [SerializeField]
        private float everyLetterDelay = 0.1f;

        private string finalText;
        private string currentText;

        public const string AlphaTag = "<color=#00000000>";

        private void OnEnable()
        {
            Detector.OnTextChanged += TextChangeDetector_OnTextChanged;
        }

        private void TextChangeDetector_OnTextChanged(string newText)
        {
            if (newText == finalText || newText == currentText)
                return;

            StopAllCoroutines();
            finalText = newText;
            StartCoroutine(LettersDisplayingCo());
        }

        private IEnumerator LettersDisplayingCo()
        {
            var wait = new WaitForSeconds(everyLetterDelay); 
            int charactersCount = finalText.Length;
            for (int i = 1; i < charactersCount; i++)
            {
                while (char.IsWhiteSpace(finalText[i]))
                    i++;

                currentText = finalText.Insert(i, AlphaTag);
                Detector.Label.text = currentText;
                yield return wait;
            }
            currentText = finalText;
            Detector.Label.text = currentText;
        }

        private void OnDisable()
        {
            Detector.OnTextChanged -= TextChangeDetector_OnTextChanged;
        }
    }
}
