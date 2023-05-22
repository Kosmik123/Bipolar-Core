#if ENABLE_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bipolar.Input.InputSystem
{
    public class InputSystemMoveInputProvider : MonoBehaviour, IMoveInputProvider
    {
        [SerializeField]
        private InputActionReference moveAction;

        Vector2 IMoveInputProvider.GetMotion()
        {
            return moveAction.action.ReadValue<Vector2>();
        }
    }
}
#endif
