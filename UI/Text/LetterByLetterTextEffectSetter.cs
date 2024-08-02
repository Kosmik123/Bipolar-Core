using System;
using TMPro;
using UnityEngine;

namespace Bipolar.UI
{
    [RequireComponent (typeof(TextChangeDetector), typeof (LetterByLetterTextEffect))]
    public class LetterByLetterTextEffectSetter : MonoBehaviour
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

        private LetterByLetterTextEffect letterByLetterTextEffect;
        public LetterByLetterTextEffect LetterByLetterTextEffect
        {
            get
            {
                if (letterByLetterTextEffect == null)
                    letterByLetterTextEffect = GetComponent<LetterByLetterTextEffect>();
                return letterByLetterTextEffect;
            }
        }

        private void OnEnable()
        {
            Detector.OnTextChanged += TextChangeDetector_OnTextChanged;
        }

		private void TextChangeDetector_OnTextChanged(string text)
        {
            LetterByLetterTextEffect.SetText(text);
        }

        private void OnDisable()
        {
            Detector.OnTextChanged -= TextChangeDetector_OnTextChanged;
        }
    }


}
