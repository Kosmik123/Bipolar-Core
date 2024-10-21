#if ENABLE_INPUT_SYSTEM
using UnityEngine;

namespace Bipolar.Input.InputSystem
{
    [AddComponentMenu(AddComponentMenuPath.Input + "Input System Movement Input Provider")]
    public class MovementInputProvider : InputProviderBase, IMoveInputProvider
    {
		public Vector2 GetMovement() => inputActionInstance.action.ReadValue<Vector2>();
	}
}
#endif
