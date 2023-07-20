using UnityEngine;

namespace Bipolar.UI.Controls
{
    public class GlobalChoiceOptionsController : ChoiceOptionsController
    {
        [SerializeField]
        private ChoiceOptions options;

        public override int OptionsCount => options.Count;

        public override string GetValueString(int index)
        {
            return options.Get(index);
        }
    }
}
