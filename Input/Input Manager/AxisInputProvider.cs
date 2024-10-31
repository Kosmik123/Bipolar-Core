using UnityEngine;

namespace Bipolar.Input.InputManager
{
	[AddComponentMenu(AddComponentMenuPath.Input + "Input Manager Axis Input Provider")]
    public class AxisInputProvider : MonoBehaviour, IAxisInputProvider
    {
#if NAUGHTY_ATTRIBUTES
        [NaughtyAttributes.InputAxis]
#endif
        [SerializeField]
        private string axis;
        [SerializeField]
        private bool rawInput;

        public float GetAxis() => GetAxis(axis, rawInput);

        public static float GetAxis(string axis, bool rawInput = false)
        {
            if (string.IsNullOrEmpty(axis))
                return 0;

            return rawInput ? UnityEngine.Input.GetAxisRaw(axis) : UnityEngine.Input.GetAxis(axis);
        }
    }
}
