using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Bipolar.UI
{
    public class Button : UnityEngine.UI.Button
    {
        public event System.Action OnClicked;
        
        [SerializeField]
        private TMP_Text label;

        public string Text
        {
            get => label ? label.text : string.Empty;
            set
            {
                if (label)
                    label.text = value;
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            OnClicked?.Invoke();
        }
    }
}
