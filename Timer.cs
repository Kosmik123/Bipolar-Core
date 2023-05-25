using UnityEngine;
using System.Collections;
#if NAUGHTY_ATTRIBUTES
using NaughtyAttributes;
#endif

namespace Bipolar.Core
{
    [System.Serializable]
    public abstract class Timer
    {
        public event System.Action OnEnded;

        [SerializeField]
        private float speed = 1;
        [SerializeField]
        private float duration;

        private MonoBehaviour owner;
        private Coroutine coroutine;

        [SerializeField]
        protected float time;
        public float CurrentTime
        {
            get => time;
            set
            {
                time = value;
            }
        }

        protected Timer(MonoBehaviour owner)
        {
            this.owner = owner;
            Reset();
        }

        private IEnumerator UpdateCo()
        {
            while (true)
            {
                yield return null;
                time += Time.deltaTime;




            }
        }

        public void Reset()
        {
            time = 0;
        }
    }
}
