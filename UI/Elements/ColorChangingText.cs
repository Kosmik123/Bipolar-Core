using UnityEngine;

namespace Bipolar.UI
{
    internal class ColorChangingText : ChangingText // wątpliwa przydatność
    {
        [Space(20)]
        [SerializeField]
        protected float duration;

        [SerializeField]
        private Gradient colorChange;

        [Header("States")]
        [SerializeField]
        private float progress = 0;

        [SerializeField]
        private float waitTime = 0;

        private void Start()
        {
            Refresh();
        }

        private void OnEnable()
        {
            Reset();
        }

        public void Reset()
        {
            waitTime = 0;
            progress = 0;
            Refresh();
        }

        private void Update()
        {
            if (waitTime < waitDuration)
            {
                waitTime += Time.deltaTime;
                return;
            }

            progress += Time.deltaTime / duration;
            if (progress > 1)
            {
                progress = isLooped ? 0 : 1;
                Refresh();
            }
            else if (progress < 1)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            label.color = colorChange.Evaluate(progress);
        }
    }
}
