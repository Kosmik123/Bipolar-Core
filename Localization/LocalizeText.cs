using Bipolar.UI;
using UnityEngine;
#if UNITY_LOCALIZATION
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Bipolar.Localization
{
    [RequireComponent(typeof(TextChangeDetector))]
    public abstract class LocalizeText : MonoBehaviour
    {
        [SerializeField]
        protected TextChangeDetector textChangeDetector;

        protected virtual void Reset()
        {
            textChangeDetector = GetComponent<TextChangeDetector>();
        }

        protected abstract void RefreshLocalizedText(string text);

        private void OnEnable()
        {
            textChangeDetector.OnTextChanged += RefreshLocalizedText;
        }

        private void OnDisable()
        {
            textChangeDetector.OnTextChanged -= RefreshLocalizedText;
        }
    }

    public abstract class LocalizeText<T> : LocalizeText where T : MonoBehaviour
    {
        [SerializeField]
        protected T localizeEvent;

        protected override void Reset()
        {
            base.Reset();
            localizeEvent = GetComponent<T>();
        }
    }
}
#endif
