using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;



namespace Bipolar.Localization
{
    // Comment the class you are not using. Uncomment class you are using

    using Label2 = UnityEngine.UI.Text;
    using Label = TMPro.TMP_Text;

    public class LocalizeText : MonoBehaviour
    {
        [SerializeField]
        private Label label;
        [SerializeField]
        private LocalizeStringEvent localizeStringEvent;



    }
}
