using UnityEngine;
using UnityEngine.Events;
#if NAUGHTY_ATTRIBUTES
using NaughtyAttributes;
#endif

namespace Bipolar
{
    public class Timer : MonoBehaviour
    {
        public event System.Action OnElapsed;

        [SerializeField, Min(0)]
        private float speed = 1;
        public float Speed
        {
            get => speed;
            set
            {
                speed = value;
            }
        }

        [SerializeField, Min(0)]
        private float duration;
        public float Duration
        {
            get => duration;
            set
            {
                duration = value;
            }
        }

        [SerializeField]
        private bool autoReset;
        public bool AutoReset
        {
            get => autoReset;
            set
            {
                autoReset = value;
            }
        }

#if NAUGHTY_ATTRIBUTES
        [ReadOnly]
#endif
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

        [SerializeField]
        private UnityEvent onElapsed;

        private void Update()
        {
            time += speed * Time.deltaTime;
            if (time >= duration)
            {
                time = 0;
                if (autoReset == false)
                    enabled = false;

                OnElapsed?.Invoke();
                onElapsed.Invoke();
            }
        }
    }
}
