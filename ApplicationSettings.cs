using UnityEngine;
#if NAUGHTY_ATTRIBUTES
using NaughtyAttributes;
#endif

namespace Bipolar
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
