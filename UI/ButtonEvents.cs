using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.UI
{
	[RequireComponent(typeof(Button))]
	public class ButtonEvents : MonoBehaviour
    {
        private Button _button;
        public Button Button => this.GetRequired(ref _button);

        [SerializeField]
        private UnityEvent onHover;
        [SerializeField]
        private UnityEvent onUnhover;

		private void OnEnable()
		{
			Button.OnHighlightChanged += Button_OnHighlightChanged;
		}

		private void Button_OnHighlightChanged(Button button, bool highlighted)
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
