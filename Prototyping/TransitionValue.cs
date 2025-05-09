using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.Prototyping
{
    public abstract class Transitioner : MonoBehaviour
    {
        [SerializeField]
        protected float transitionDuration;

        [SerializeField]
        protected bool transitionOnStart;
        [SerializeField]
        protected UnityEvent onTransitionEnd;

        protected void Start()
        {
            if (transitionOnStart)
                StartTransition();
        }

        [ContextMenu("Start Transition")]
        public void StartTransition()
        {
            StartCoroutine(TransitionCo());
        }

        private IEnumerator TransitionCo()
        {
            for (float timer = 0; timer < transitionDuration; timer += Time.deltaTime)
            {
                float progress = timer / transitionDuration;
                ApplyTransition(progress);
                yield return null;
            }
            ApplyTransition(1);
            onTransitionEnd.Invoke();
        }

        protected abstract void ApplyTransition(float progress);
    }

    public class TransitionColor : Transitioner
    {
        [SerializeField]
        private UnityEvent<Color> transitionedValue;
        [SerializeField]
        private Gradient transitionGradient;

        protected override void ApplyTransition(float progress)
        {
            var value = transitionGradient.Evaluate(progress);
            transitionedValue.Invoke(value);
        }
    }

    public class TransitionValue : Transitioner
    {
        [SerializeField]
        private UnityEvent<float> transitionedValue;
        [SerializeField]
        private AnimationCurve transitionCurve;
        [SerializeField]
        private float valueMultiplier;

        protected override void ApplyTransition(float progress)
        {
            transitionedValue.Invoke(valueMultiplier * transitionCurve.Evaluate(progress));
        }
    }
}
