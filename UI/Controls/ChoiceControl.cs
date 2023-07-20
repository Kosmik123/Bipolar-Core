using Bipolar.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Bipolar.UI.Controls
{
    [RequireComponent(typeof(NamedControl))]
    public class ChoiceControl : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField]
        internal TextMeshProUGUI choiceLabel;

        [Space(10), SerializeField]
        internal Button leftButton;
        [SerializeField]
        internal Button rightButton;

        [Header("To Link")]
        [SerializeField]
        internal ChoiceOptionsController optionController;

        internal int OptionsCount => optionController.OptionsCount;

        private void OnEnable()
        {
            leftButton.OnClicked += SwitchLeft;
            rightButton.OnClicked += SwitchRight;
            optionController.OnValueChanged += Refresh;
            Refresh(optionController.Index);
        }

        private void Start()
        {
            Refresh(optionController.Index);
        }

        private void SwitchRight()
        {
            Switch(+1);
        }

        private void SwitchLeft()
        {
            Switch(-1);
        }

        private void Switch(int dir)
        {
            int newIndex = optionController.Index + dir;
            bool lessThanZero = newIndex < 0;
            bool moreThanCount = newIndex >= OptionsCount;
            if (optionController.IsLooped)
                newIndex = lessThanZero ? OptionsCount - 1 : moreThanCount ? 0 : newIndex;
            else
                newIndex = lessThanZero ? 0 : moreThanCount ? OptionsCount - 1 : newIndex;
            optionController.Index = newIndex;
        }

        private void Refresh(int index)
        {
            string text = optionController.GetValueString(index);
            choiceLabel.text =  text;
        }

        private void OnDisable()
        {
            leftButton.OnClicked -= SwitchLeft;
            rightButton.OnClicked -= SwitchRight;
            optionController.OnValueChanged -= Refresh;
        }
    }
}
