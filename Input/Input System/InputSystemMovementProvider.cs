#if ENABLE_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bipolar.Input.InputSystem
{
    public class InputSystemMovementProvider : MonoBehaviour, IMoveInputProvider
    {
        [SerializeField]
        private InputActionReference moveAction;
        private InputActionReference moveActionInstance;

        private void Awake()
        {
            moveActionInstance = Instantiate(moveAction);
        }

        private void OnEnable()
        {
            moveActionInstance.action.Enable();
        }

        Vector2 IMoveInputProvider.GetMotion()
        {
            return moveActionInstance.action.ReadValue<Vector2>();
        }

        private void OnDisable()
        {
            moveActionInstance.action.Disable();
        }
    }
}
#endif
