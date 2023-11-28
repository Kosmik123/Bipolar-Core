using UnityEngine;

namespace Bipolar
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField]
        private Vector3 rotationSpeed;

        private void Update()
        {
            float dt = Time.deltaTime;
            transform.Rotate(dt * rotationSpeed);
        }
    }
}
