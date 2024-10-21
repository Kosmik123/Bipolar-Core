using UnityEngine;

namespace Bipolar.UI
{
    [AddComponentMenu(AddComponentMenuPath.UI + "Window")]
	public class Window : MonoBehaviour
    {
        public event System.Action<Window> OnClosed;

        [SerializeField]
        private Button closeButton;

        private void OnEnable()
        {
            if (closeButton)
                closeButton.OnClicked += Close; 
        }

        private void Close(Button button)
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if (closeButton)
                closeButton.OnClicked -= Close;
            OnClosed?.Invoke(this);
        }
    }
}
