#if ENABLE_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bipolar.Input.InputSystem
{
	public class InputProviderBase : MonoBehaviour
    {
		[SerializeField]
		protected InputActionReference inputAction;
        protected InputActionReference inputActionInstance;

        protected virtual void OnEnable()
        {
            inputActionInstance = Instantiate(inputAction);
            inputAction.action.Enable();
        }

        protected virtual void OnDisable()
        {
			inputActionInstance.action.Disable();
			inputActionInstance = null;
		}
	}
}
#endif
