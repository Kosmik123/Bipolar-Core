using UnityEngine;

namespace Bipolar.UI
{
	public class GlobalChoiceOptionsController : ChoiceOptionsController
	{
		[SerializeField]
		private ChoiceOptions options;
		[SerializeField]
		private BoolOverride isLooped;

		public override bool IsLooped => isLooped.TryOverride(options.IsLooped);

		public override int OptionCount => options.Count;

		public override string GetOption(int index)
		{
			return options.Get(index);
		}
	}
}
