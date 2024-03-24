#if ENABLE_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bipolar.Input.InputSystem
{
    public class InputSystemAxisInputProvider : MonoBehaviour, IAxisInputProvider
    {
        private enum AxisType
        {
            FloatValue,
            AxisX,
            AxisY,
        }
        
        [SerializeField]
        private InputActionReference inputAction;
        
        [SerializeField]
        private AxisType axis;

        public float GetAxis()
        {
            float value = axis switch
            {
                AxisType.AxisX => inputAction.action.ReadValue<Vector2>().x,
                AxisType.AxisY => inputAction.action.ReadValue<Vector2>().y,
                _ => inputAction.action.ReadValue<float>(),
            };
            return value;
        }
    }
}
#endif
