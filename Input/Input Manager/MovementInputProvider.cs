using UnityEngine;

namespace Bipolar.Input.InputManager
{
	[AddComponentMenu(AddComponentMenuPath.Input + "Input Manager Movement Input Provider")]
	public class MovementInputProvider : MonoBehaviour, IMoveInputProvider
	{
#if NAUGHTY_ATTRIBUTES
        [NaughtyAttributes.InputAxis]
#endif
        [SerializeField]
        private string horizontalAxis = "Horizontal";
        
#if NAUGHTY_ATTRIBUTES
        [NaughtyAttributes.InputAxis]
#endif
        [SerializeField]
        private string verticalAxis = "Vertical";

        [SerializeField]
        private bool rawInput;

		public Vector2 GetMovement() => new Vector2(GetAxis(horizontalAxis), GetAxis(verticalAxis));

		private float GetAxis(string axisName) => AxisInputProvider.GetAxis(axisName, rawInput);
    }
}
