using UnityEngine;

namespace Bipolar
{
    [RequireComponent(typeof(Renderer))]
    public class MaterialRandomizer : Randomizer<MeshRenderer>
    {
        [System.Serializable]
        public class RandomMaterialData
        {
            [field: SerializeField]
            public int Index { get; private set; }

            [field: SerializeField]
            public Material[] Materials { get; private set; }
        }

        [SerializeField]
        private RandomMaterialData[] materials;

        [ContextMenu("Randomize")]
        public override void Randomize()
        {
            var materialsArray = RandomizedComponent.materials;
            foreach (var data in materials)
            {
                if (data.Index >= materialsArray.Length)
                    continue;

                materialsArray[data.Index] = data.Materials[Random.Range(0, data.Materials.Length)];
            }
            RandomizedComponent.materials = materialsArray;
        }
    }
}
