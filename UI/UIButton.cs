using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Bipolar.UI
{
	public class UIButton : UnityEngine.UI.Button
    {
        public event System.Action<UIButton> OnClicked;
        public event System.Action<UIButton, bool> OnHighlightChanged;

        [SerializeField]
        private TMP_Text label;
        public TMP_Text Label => label;

        public string Text
        {
            get => label ? label.text : string.Empty;
            set
            {
                if (label)
                    label.text = value;
            }
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            label = GetComponentInChildren<TMP_Text>();
        }
#endif

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            OnClicked?.Invoke(this);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            OnHighlightChanged?.Invoke(this, true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            OnHighlightChanged?.Invoke(this, false);
        }
    }
}
