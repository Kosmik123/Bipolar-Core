using UnityEngine;

namespace Bipolar.Input
{
    public class MoveInputRotator : MonoBehaviour, IMoveInputProvider
    {
        [SerializeField]
        private Object moveInputProvider;
        public IMoveInputProvider MoveInputProvider
        {
            get => moveInputProvider as IMoveInputProvider;
            set
            {
                moveInputProvider = (Object)value;
            }
        }

        [SerializeField]
        private Transform forwardProvider;

        public Vector2 GetMotion()
        {
            return forwardProvider.rotation * MoveInputProvider.GetMotion();
        }

        private void OnValidate()
        {
            MoveInputProvider = MoveInputProvider;
        }
    }
}
