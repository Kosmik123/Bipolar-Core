using UnityEngine;

namespace Bipolar.UI
{
	public class LocalChoiceOptionsController : ChoiceOptionsController
	{
		[SerializeField]
		private string[] options;
		[SerializeField]
		private bool isLooped;

		public override bool IsLooped => isLooped;

		public override int OptionCount => options.Length;

		public override string GetOption(int index)
		{
			return options[index];
		}
	}
}
