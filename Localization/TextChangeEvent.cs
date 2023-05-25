using UnityEngine;

namespace Bipolar.UI
{
    // Comment the class you are not using. Uncomment class you are using

    using Label = UnityEngine.UI.Text;
    //using Label = TMPro.TMP_Text;

    [RequireComponent(typeof(Label))]
    public class TextChangeEvent : MonoBehaviour
    {
        public event System.Action<Label> OnTextChanged;

        [SerializeField]
        private Label label;

        [SerializeField]
        private string currentText;


        private void Reset()
        {
            label = GetComponent<Label>();
        }

        private void Update()
        {
            if (currentText != label.text)
            {

            }
        }



    }
}
