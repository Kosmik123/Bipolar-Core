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
            var direction = MoveInputProvider.GetMotion();


            return forwardProvider.forward * direction.y + forwardProvider.right * direction.x;
        }

        private void OnValidate()
        {
            MoveInputProvider = MoveInputProvider;
        }
    }
}
