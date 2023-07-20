using System;
using UnityEngine;

namespace Bipolar.UI.Controls
{
    public class LocalChoiceOptionsController : ChoiceOptionsController
    {
        [SerializeField]
        private string[] valuesStrings;

        public override int OptionsCount => valuesStrings.Length;

        public override string GetValueString(int index)
        {
            return valuesStrings[index];
        }
    }
}
