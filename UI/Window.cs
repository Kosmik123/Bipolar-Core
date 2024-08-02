using System;
using UnityEngine;

namespace Bipolar.UI
{
	public class Window : MonoBehaviour
    {
        public event System.Action<Window> OnClosed;

        [SerializeField]
        private UIButton closeButton;

        private void OnEnable()
        {
            if (closeButton)
                closeButton.OnClicked += Close; 
        }

        private void Close(UIButton button)
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
