using System;
using UnityEngine;

namespace Bipolar.Input
{
    public abstract class InputManagerActionInputProvider : MonoBehaviour, IActionInputProvider
    {
        public event Action OnPerformed;

        [SerializeField]
        private KeyCode key;

        protected abstract Func<KeyCode, bool> CheckingMethod { get; }

        private void Update()
        {
            if (CheckingMethod(key))
            {
                OnPerformed?.Invoke();
            }
        }
    }
}
