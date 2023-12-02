using UnityEngine;
using UnityEngine.UIElements;
using UInput = UnityEngine.Input;

namespace Bipolar.Input
{
    public class InputManagerClickActionProvider : MonoBehaviour, IActionInputProvider
    {
        public event System.Action OnAction;

        [SerializeField]
        private MouseButton button;

        private bool isPressed;

        private void Update()
        {
            int button = (int)this.button;
            if (UInput.GetMouseButtonDown(button))
            {
                isPressed = true;
            }
            else if (UInput.GetMouseButtonUp(button))
            {
                if (isPressed)
                    OnAction?.Invoke();
                isPressed = false;
            }
        }
    }
}
