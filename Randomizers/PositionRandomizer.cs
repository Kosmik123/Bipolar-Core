using UnityEngine;

namespace Bipolar
{
    public class PositionRandomizer : GenericRandomizer<Transform>
    {
        [SerializeField]
        private Vector3 minPosition = -Vector3.one;
        [SerializeField]
        private Vector3 maxPosition = Vector3.one;

        [SerializeField]
        private Space space;

        [ContextMenu("Randomize")]
        public override void Randomize()
        {
            var randomPosition = new Vector3(
                Random.Range(minPosition.x, maxPosition.x),
                Random.Range(minPosition.y, maxPosition.y),
                Random.Range(minPosition.z, maxPosition.z));

            if (space == Space.World)
                RandomizedComponent.position = randomPosition;
            else
                RandomizedComponent.localPosition = randomPosition;
        }
    }
}
