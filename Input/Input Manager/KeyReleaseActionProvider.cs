using System;
using UnityEngine;

namespace Bipolar.Input.InputManager
{
	[AddComponentMenu(AddComponentMenuPath.Input + "Input Manager Key Release Input Provider")]
    public class KeyReleaseActionInputProvider : KeyActionInputProvider
    {
		protected override Func<KeyCode, bool> GetCheckingMethod()
		{
			return (key) => UnityEngine.Input.GetKeyUp(key);
		}
	}
}
