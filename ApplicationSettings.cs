using UnityEngine;
using NaughtyAttributes;

namespace Bipolar.Core
{
    public class ApplicationSettings : MonoBehaviour
    {
        [SerializeField]
        private int targetFrameRate;

        private void Awake()
        {
            ApplySettings();
        }

        [ContextMenu("Apply Settings")]
#if NAUGHTY_ATTRIBUTES
        [Button]
#endif
        private void ApplySettings()
        {
            Application.targetFrameRate = targetFrameRate;
        }
    }
}
