using System;
using UnityEngine;

namespace Bipolar.Input
{
    public class InputManagerPressActionInputProvider : InputManagerActionInputProvider
    {
        protected override Func<KeyCode, bool> CheckingMethod => (key) => UnityEngine.Input.GetKeyDown(key);
    }
}
