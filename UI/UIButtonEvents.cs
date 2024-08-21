using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.UI
{
	[RequireComponent(typeof(UIButton))]
	public class UIButtonEvents : MonoBehaviour
    {
        private UIButton _button;
        public UIButton Button => this.GetRequired(ref _button);

        [SerializeField]
        private UnityEvent onHover;
        [SerializeField]
        private UnityEvent onUnhover;

		private void OnEnable()
		{
			Button.OnHighlightChanged += Button_OnHighlightChanged;
		}

		private void Button_OnHighlightChanged(UIButton button, bool highlighted)
		{
            var @event = highlighted ? onHover : onUnhover;
            @event.Invoke();
		}

		private void OnDisable()
		{
			Button.OnHighlightChanged -= Button_OnHighlightChanged;
		}
	}
}
