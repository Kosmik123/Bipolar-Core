#if ENABLE_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bipolar.Input.InputSystem
{
    public class InputActionEnabler : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference inputAction;

        private void OnEnable()
        {
            inputAction.action.Enable();
        }

        private void OnDisable()
        {
            inputAction.action.Disable();
        }
    }
}
#endif
