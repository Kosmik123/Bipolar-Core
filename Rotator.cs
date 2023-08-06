using UnityEngine;

namespace Bipolar.Core
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField]
        private Vector3 axis;

        [SerializeField]
        private float speed;

        [SerializeField]
        private Space space;

        private void Update()
        {
            transform.Rotate(speed * Time.deltaTime * axis, space);
        }
    }
}
