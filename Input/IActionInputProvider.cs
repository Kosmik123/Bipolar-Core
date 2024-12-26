using System;

namespace Bipolar.Input
{
    public interface IActionInputProvider
    {
        event Action OnPerformed;
    }

	[Serializable]
	public class ActionInputProvider : Serialized<IActionInputProvider>, IActionInputProvider
	{
		public event Action OnPerformed
		{
			add => Value.OnPerformed += value;
			remove => Value.OnPerformed -= value;
		}
	}
}
