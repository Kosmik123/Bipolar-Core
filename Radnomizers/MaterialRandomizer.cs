using UnityEngine;

namespace Bipolar
{
    [RequireComponent(typeof(Renderer))]
    public class MaterialRandomizer : MonoBehaviour
    {
        [System.Serializable]
        public class RandomMaterialData
        {
            [field: SerializeField]
            public int Index { get; private set; }

            [field: SerializeField]
            public Material[] Materials { get; private set; }
        }

        private Renderer _renderer;
        public Renderer Renderer
        {
            get
            {
                if (_renderer == null)
                    _renderer = GetComponent<Renderer>();
                return _renderer;
            }
        }

        [SerializeField]
        private RandomMaterialData[] materials;

        private void Awake()
        {
            Randomize();
        }

        [ContextMenu("Randomize")]
        private void Randomize()
        {
            var materialsArray = Renderer.materials;
            foreach (var data in materials)
            {
                if (data.Index >= materialsArray.Length)
                    continue;

                materialsArray[data.Index] = data.Materials[Random.Range(0, data.Materials.Length)];
            }
            Renderer.materials = materialsArray;
        }
    }
}
