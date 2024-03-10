using System.Collections;
using TMPro;
using UnityEngine;

namespace Bipolar.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class LetterByLetterTextEffect : MonoBehaviour
    {
        private TMP_Text label;
        public TMP_Text Label
        {
            get
            {
                if (label == null)
                    label = GetComponent<TMP_Text>();
                return label;
            }
        }

        [SerializeField]
        private float everyLetterDelay = 0.05f;

        private string finalText;
        private string currentText;

        public bool IsShowing => currentText != finalText;

        public const string AlphaTag = "<color=#00000000>";

        private void OnEnable()
        {
            SetText(Label.text);
        }

        public void SetText(string newText)
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

                Label.text = currentText = finalText.Insert(i, AlphaTag);
                yield return wait;
            }
            Label.text = currentText = finalText;
        }

        public void ForceEnd()
        {
            StopAllCoroutines();
            Label.text = currentText = finalText;
        }
    }
}
