using UnityEngine;

namespace Bipolar
{
    [RequireComponent(typeof(MeshFilter))]
    public class MeshRandomizer : Randomizer<MeshFilter>
    {
        [SerializeField]
        private Mesh[] meshes;

        [ContextMenu("Randomize")]
        public override void Randomize()
        {
            RandomizedComponent.mesh = meshes.GetRandom();
        }
    }
}
