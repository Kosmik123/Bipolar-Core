using UnityEngine;

namespace Bipolar
{
    public class ScaleRandomizer : GenericRandomizer<Transform>
    {
        [SerializeField]
        private Vector3 minScale = Vector3.one;
        [SerializeField]
        private Vector3 maxScale = Vector3.one * 2;

        [ContextMenu("Randomize")]
        public override void Randomize()
        {
            RandomizedComponent.localScale = new Vector3(
                Random.Range(minScale.x, maxScale.x),
                Random.Range(minScale.y, maxScale.y),
                Random.Range(minScale.z, maxScale.z));
        }
    }
}
