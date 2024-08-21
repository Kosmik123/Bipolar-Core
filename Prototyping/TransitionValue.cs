using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.Prototyping
{
    public class TransitionValue : MonoBehaviour
    {
        [SerializeField]
        private float transitionDuration;
        [SerializeField]
        private UnityEvent<float> transitionedValue;
        [SerializeField]
        private AnimationCurve transitionCurve;
        [SerializeField]
        private float valueMultiplier;

        [SerializeField]
        private bool transitionOnStart;
        [SerializeField]
        private UnityEvent onTransitionEnd;

        private void Start()
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
                float value = valueMultiplier * transitionCurve.Evaluate(progress);
                transitionedValue.Invoke(value);
                yield return null;
            }
            transitionedValue.Invoke(valueMultiplier * transitionCurve.Evaluate(1));
            onTransitionEnd.Invoke();
        }
    }
}
