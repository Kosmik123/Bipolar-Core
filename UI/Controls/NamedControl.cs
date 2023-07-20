using TMPro;
using UnityEngine;

namespace Bipolar.UI.Controls
{
    [RequireComponent(typeof(RectTransform))]
    public class NamedControl : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField]
        private TextMeshProUGUI nameLabel;

        [Header("Settings")]
        [SerializeField]
        private new string name;

        private void OnValidate()
        {
            nameLabel.text = name;
        }
    }
}
