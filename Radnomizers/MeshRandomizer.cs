using UnityEngine;

namespace Bipolar
{
    [RequireComponent(typeof(MeshFilter))]
    public class MeshRandomizer : MonoBehaviour
    {
        private MeshFilter filter;
        public MeshFilter MeshFilter
        {
            get
            {
                if (filter == null)
                    filter = GetComponent<MeshFilter>();
                return filter;
            }
        }

        [SerializeField]
        private Mesh[] meshes;

        private void Awake()
        {
            Randomize();
        }

        [ContextMenu("Randomize")]
        private void Randomize()
        {
            MeshFilter.mesh = meshes[Random.Range(0, meshes.Length)];
        }
    }
}
