#if ENABLE_INPUT_SYSTEM
using UnityEngine;

namespace Bipolar.Input.InputSystem
{
    [AddComponentMenu(AddComponentMenuPath.Input + "Input System Axis Input Provider")]
	public class AxisInputProvider : InputProviderBase, IAxisInputProvider
    {
        private enum AxisType
        {
            FloatValue,
            AxisX,
            AxisY,
        }
        
        [SerializeField]
        private AxisType axis;

		public float GetAxis() => axis switch
		{
			AxisType.AxisX => inputAction.action.ReadValue<Vector2>().x,
			AxisType.AxisY => inputAction.action.ReadValue<Vector2>().y,
			_ => inputAction.action.ReadValue<float>(),
		};
	}
}
#endif
