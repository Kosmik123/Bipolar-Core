using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Bipolar.UI
{
    internal class ValueChangingText : ChangingText // wątpliwa przydatność
    {
        [Space(20)]
        [SerializeField]
        private string prefix;
        [SerializeField]
        private string[] changingTexts; 
        [SerializeField]
        private string suffix;

        // STATES
        private float changeTime = 0;
        private int index = 0;

        private void Update()
        {
            if (index >= changingTexts.Length - 1 && isLooped == false)
                return;

            if (Time.time > changeTime)
            {
                index = (index + 1) % changingTexts.Length;
                string text = GetText(index);
                label.text = text;
                changeTime = Time.time + waitDuration;
            }
        }

        private string GetText(int index)
        {
            var locale = LocalizationSettings.SelectedLocale;
            var localizedPrefix = LocalizationSettings.StringDatabase.GetLocalizedString("Words", prefix, locale);
            return $"{localizedPrefix}{changingTexts[index]}{suffix}";
        }

        private void OnValidate()
        {
            label.text = GetText(0);
        }

    }
}
