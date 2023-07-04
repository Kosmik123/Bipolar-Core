using UnityEngine;
using System.Collections;
#if NAUGHTY_ATTRIBUTES
using NaughtyAttributes;
#endif

namespace Bipolar.Core
{
    [System.Serializable]
    public class Timer
    {
        public event System.Action OnEnded;

        [SerializeField]
        private float speed = 1;
        public float Speed
        {
            get => speed;
            set
            {
                speed = value;
            }
        }

        [SerializeField]
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
        protected float time;
        public float CurrentTime
        {
            get => time;
            set
            {
                time = value;
            }
        }

        public float Progress
        { 
            get
            {
                if (isProgressCalculatedThisFrame == false)
                {
                    isProgressCalculatedThisFrame = true;
                    progressCalculated = time / duration;
                }
                return progressCalculated;
            }
        }
        private float progressCalculated;
        private bool isProgressCalculatedThisFrame;

        private MonoBehaviour owner;
        private Coroutine coroutine;

        public Timer(MonoBehaviour owner, float duration = 1, float speed = 1)
        {
            Init(owner);
            Duration = duration;
            Speed = speed;
        }

        protected void Init(MonoBehaviour owner)
        {
            if (owner == null)
                throw new System.ArgumentNullException();
            this.owner = owner;
            Reset();
        }

        public void Reset()
        {
            time = 0;
            StopCounting();
        }

        public void Start()
        {
            StopCounting();
            coroutine = owner.StartCoroutine(UpdateCo());
        }

        private void StopCounting()
        {
            if (coroutine != null)
                owner.StopCoroutine(coroutine);
        }

        public void Pause()
        {
            StopCounting();
        }

        private IEnumerator UpdateCo()
        {
            while (true)
            {
                yield return null;
                time += speed * Time.deltaTime;
                isProgressCalculatedThisFrame = false;
                if (HasEnded())
                    break;
            }
            OnEnded?.Invoke();
        }

        private bool HasEnded()
        {
            return speed < 0 ? time <= 0 : time >= duration;
        }
    }
}
