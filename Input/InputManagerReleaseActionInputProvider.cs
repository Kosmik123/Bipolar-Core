using System;
using UnityEngine;

namespace Bipolar.Input
{
    public class InputManagerReleaseActionInputProvider : InputManagerActionInputProvider
    {
        protected override Func<KeyCode, bool> CheckingMethod => (key) => UnityEngine.Input.GetKeyUp(key);
    }
}
