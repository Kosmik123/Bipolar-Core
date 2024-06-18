using UnityEngine;

namespace Bipolar.UI
{
    // Comment the class you are NOT using. Uncomment the class you are using
    //using Label = UnityEngine.UI.Text;
    using Label = TMPro.TMP_Text;

    [RequireComponent(typeof(Label))]
    public class TextChangeDetector : MonoBehaviour
    {
        public delegate void TextChangeHandler(string newText);

        public event TextChangeHandler OnTextChanged;

        private Label _label;
        public Label Label
        {
            get
            {
                if (_label == null)
                    _label = GetComponent<Label>();
                return _label;
            }
        }

        private string currentText;

        private void OnEnable()
        {
            Label.RegisterDirtyVerticesCallback(DetectTextChange);
            DetectTextChange();
        }

        private void DetectTextChange()
        {
            currentText = Label.text;
            OnTextChanged?.Invoke(currentText);
        }

        private void OnDisable()
        {
            Label.UnregisterDirtyVerticesCallback(DetectTextChange);
        }
    }

    public static class LabelExtensions
    {
    }
}
