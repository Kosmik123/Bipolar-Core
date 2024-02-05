using UnityEngine;

namespace Bipolar
{
    public abstract class Randomizer<T> : MonoBehaviour where T : Component
    {
        private T randomizedComponent;
        public T RandomizedComponent
        {
            get
            {
                if (randomizedComponent == null)
                    randomizedComponent = GetComponent<T>();
                return randomizedComponent;
            }
        }

        [SerializeField]
        private bool randomizeOnAwake = true;
        public bool RandomizeOnAwake
        {
            get => randomizeOnAwake;
            set => randomizeOnAwake = value;
        }

        public abstract void Randomize();

        private void Awake()
        {
            if (randomizeOnAwake)
                Randomize();
        }
    }
}
