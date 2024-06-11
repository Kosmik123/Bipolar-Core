using UnityEngine;

namespace Bipolar.Prototyping
{
    public class Rotator : MonoBehaviour
    {
        [field: SerializeField]
        public Vector3 RotationSpeed { get; set; }

        private void Update()
        {
            float dt = Time.deltaTime;
            transform.Rotate(dt * RotationSpeed);
        }
    }
}
