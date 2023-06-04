using UnityEngine;

namespace Bipolar.UI
{
    // Comment the class you are not using. Uncomment the class you are using
    //using Label = UnityEngine.UI.Text;
    using Label = TMPro.TMP_Text;

    [RequireComponent(typeof(Label))]
    public class TextChangeDetector : MonoBehaviour
    {
        public event System.Action<string> OnTextChanged;

        [SerializeField, HideInInspector]
        private Label label;

        [SerializeField]
        private string currentText;

        private void Reset() => FillReference();

        private void Awake()
        {
            if (label == null)
                FillReference();
        }

        private void FillReference()
        {
            label = GetComponent<Label>();
        }

        private void OnEnable()
        {
            label.RegisterDirtyVerticesCallback(DetectTextChange);
            DetectTextChange();
        }

        private void DetectTextChange()
        {
            currentText = label.text;
            OnTextChanged?.Invoke(currentText);
        }

        private void OnDisable()
        {
            label.UnregisterDirtyVerticesCallback(DetectTextChange);
        }
    }
}
