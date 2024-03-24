using UnityEngine;

namespace Bipolar.Input
{
    public abstract class MovementInputProvider : MonoBehaviour, IMoveInputProvider
    {
        public abstract Vector2 GetMotion();
    }

    public class InputManagerMovementProvider : MovementInputProvider
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

        public override Vector2 GetMotion()
        {
            return new Vector2(GetAxis(horizontalAxis), GetAxis(verticalAxis));
        }

        private float GetAxis(string axisName) => InputManagerAxisInputProvider.GetAxis(axisName, rawInput);
    }
}
