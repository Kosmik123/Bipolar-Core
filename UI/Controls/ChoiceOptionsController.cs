using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.UI
{
	public abstract class ChoiceOptionsController : MonoBehaviour
	{
		public event System.Action<int> OnOptionChanged;

		public abstract string GetOption(int index);
		public abstract int OptionCount { get; }

		public abstract bool IsLooped { get; }

		[SerializeField]
		private int index;
		public int Index
		{
			get => index;
			set
			{
				if (IsLooped)
				{
					value %= OptionCount;
					value += OptionCount;
					value %= OptionCount;
				}
				else
				{
					value = Mathf.Clamp(value, 0, OptionCount - 1);
				}

				index = value;
				onOptionChange.Invoke(index);
				OnOptionChanged?.Invoke(index);
			}
		}

		[SerializeField]
		private UnityEvent<int> onOptionChange;
	}
}
