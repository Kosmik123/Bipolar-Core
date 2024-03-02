using System;
using UnityEngine;

namespace Bipolar.Input
{
    public abstract class InputManagerActionInputProvider : MonoBehaviour, IActionInputProvider
    {
        public event Action OnPerformed;

        [SerializeField]
        private KeyCode[] keys;

        protected abstract Func<KeyCode, bool> CheckingMethod { get; }

        private void Update()
        {
            foreach (var key in keys)
            {
                if (CheckingMethod(key))
                {
                    OnPerformed?.Invoke();
                    return;
                }
            }
        }
    }
}
