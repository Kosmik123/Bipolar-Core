using UnityEngine;
using UnityEngine.UIElements;

namespace Bipolar.Input.InputManager
{
	[AddComponentMenu(AddComponentMenuPath.Input + "Input Manager Mouse Click Input Provider")]
	public class MouseClickActionProvider : MonoBehaviour, IActionInputProvider
    {
        public event System.Action OnPerformed;

        [SerializeField]
        private MouseButton button;

        private bool isPressed;

        private void Update()
        {
            int button = (int)this.button;
            if (UnityEngine.Input.GetMouseButtonDown(button))
            {
                isPressed = true;
            }
            else if (UnityEngine.Input.GetMouseButtonUp(button))
            {
                if (isPressed)
                    OnPerformed?.Invoke();
                isPressed = false;
            }
        }
    }
}
