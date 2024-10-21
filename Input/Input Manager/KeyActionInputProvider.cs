using System;
using UnityEngine;

namespace Bipolar.Input.InputManager
{
	public abstract class KeyActionInputProvider : MonoBehaviour, IActionInputProvider
    {
        public event Action OnPerformed;

        [SerializeField]
        private KeyCode[] keys;

		private Func<KeyCode, bool> checkingMethod;

		protected abstract Func<KeyCode, bool> GetCheckingMethod();

		private void OnEnable()
		{
            checkingMethod = GetCheckingMethod();
		}

		private void Update()
        {
            foreach (var key in keys)
            {
                if (checkingMethod(key))
                {
                    OnPerformed?.Invoke();
                    return;
                }
            }
        }
    }
}
