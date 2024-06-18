using Bipolar.Input;
using TMPro;
using UnityEngine;

namespace Bipolar.UI
{
	public class ChoiceControl : MonoBehaviour
    {
        [SerializeField]
		protected TMP_Text choiceLabel;

		[Space]
        [SerializeField]
		protected UIButton leftButton;
		[SerializeField]
		protected UIButton rightButton;

        [Space]
        [SerializeField]
		protected ChoiceOptionsController optionsController;
        public int OptionsCount => optionsController.OptionCount;

		protected virtual void OnEnable()
		{
			leftButton.OnClicked += SwitchLeft;
			rightButton.OnClicked += SwitchRight;
			optionsController.OnOptionChanged += Refresh;
            Refresh(optionsController.Index);
		}

		protected virtual void Refresh(int index)
		{
			string text = optionsController.GetOption(index);
			choiceLabel.SetText(text);
		}

		protected void SwitchLeft() => Switch(-1);

		protected void SwitchRight() => Switch(+1);

        protected virtual void Switch(int dir)
        {
			int newIndex = optionsController.Index + dir;
            optionsController.Index = newIndex;
		}

		protected virtual void OnDisable()
		{
			leftButton.OnClicked -= SwitchLeft;
			rightButton.OnClicked -= SwitchRight;
			optionsController.OnOptionChanged -= Refresh;
		}
	}
}
