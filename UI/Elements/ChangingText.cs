using UnityEngine;
using TMPro;

namespace Bipolar.UI
{
    internal abstract class ChangingText : MonoBehaviour // wątpliwa przydatność
    {
        [SerializeField]
        protected TextMeshProUGUI label;
        public TextMeshProUGUI Label => label;

        [SerializeField]
        protected bool isLooped;

        [Header("Settings")]
        [SerializeField]
        protected float waitDuration;
    }
}
