using UnityEngine;

namespace Bipolar
{
    public class UniformScaleRandomizer : GenericRandomizer<Transform>
    {
        [SerializeField]
        private float minScale = 1;
        [SerializeField]
        private float maxScale = 2;

        [ContextMenu("Randomize")]
        public override void Randomize()
        {
            float scale = Random.Range(minScale, maxScale);
            RandomizedComponent.localScale = scale * Vector3.one;
        }
    }
}
