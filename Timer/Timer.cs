using UnityEngine;
using System.Collections;
#if NAUGHTY_ATTRIBUTES
using NaughtyAttributes;
#endif

namespace Bipolar
{
    [System.Serializable]
    public struct Timer : ITimer
    {
        public System.Action OnElapsed { get; set; }

        [SerializeField, Min(0)]
        private float speed;
        public float Speed
        {
            get => speed;
            set
            {
                speed = value;
            }
        }

        [SerializeField, Min(0.0001f)]
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

        [SerializeField]
        private float time;
        public float CurrentTime
        {
            get => time;
            set
            {
                time = value;
            }
        }

        private MonoBehaviour owner;
        private Coroutine coroutine;

        public Timer(MonoBehaviour owner, float speed = 1, float duration = 1, System.Action onElapsed = null, bool autoReset = false)
        {
            this.owner = owner;
            time = 0;
            this.duration = duration;
            this.speed = speed;
            this.autoReset = autoReset;
            OnElapsed = onElapsed;
            coroutine = null;
        }

        public void Init(MonoBehaviour owner)
        {
            if (owner == null)
                throw new System.ArgumentNullException();
            this.owner = owner;
            Reset();
            Start();
        }

        public void Restart()
        {
            Reset();
            Start();
        }

        public void Reset()
        {
            time = 0;
            StopCounting();
        }

        public void Start(System.Action actionOnElapsed)
        {
            OnElapsed = actionOnElapsed;
            Start();
        }

        public void Start()
        {
            coroutine = owner.StartCoroutine(UpdateCo());
        }

        private void StopCounting()
        {
            if (coroutine != null)
            {
                owner.StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        public void Stop()
        {
            StopCounting();
        }

        private IEnumerator UpdateCo()
        {
            while (true)
            {
                yield return null;
                TimerHelper.UpdateTimer(ref time, speed, duration, OnElapsedAction);
            }
        }

        private void OnElapsedAction()
        {
            if (autoReset == false) 
                StopCounting();

            OnElapsed?.Invoke();
        }
    }
}
