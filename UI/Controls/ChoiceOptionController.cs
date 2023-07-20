using System;
using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.UI.Controls
{
    public abstract class ChoiceOptionsController : MonoBehaviour
    {
        public event Action<int> OnValueChanged;
        public abstract string GetValueString(int index);
        public abstract int OptionsCount { get; }

        [SerializeField]
        private bool isLooped;
        public bool IsLooped => isLooped;

        [SerializeField]
        private int currentIndex;
        public int Index
        {
            get => currentIndex;
            set
            {
                currentIndex = value;
                OnValueChanged?.Invoke(currentIndex);
            }
        }
    }
}
