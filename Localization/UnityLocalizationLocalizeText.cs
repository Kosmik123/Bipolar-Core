#if UNITY_LOCALIZATION
using UnityEngine;
using Bipolar.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Localization.Components;

namespace Bipolar.Localization
{
    [RequireComponent(typeof(LocalizeStringEvent), typeof(TextChangeDetector))]
    public class UnityLocalizationLocalizeText : LocalizeText<LocalizeStringEvent>
    {
        private TableEntry currentTableEntry;
        private string currentText;

        private bool hasChanged;

        protected override void RefreshLocalizedText(string text)
        {
            if (hasChanged)
                return;

            currentText = text;
            var tableReference = localizeEvent.StringReference.TableReference;
            if (tableReference.ReferenceType == TableReference.Type.Empty)
                return;

            var tableFindingOperation = LocalizationSettings.StringDatabase.GetTableAsync(tableReference);
            tableFindingOperation.Completed += OnTableFound;
        }

        private void OnTableFound(AsyncOperationHandle<StringTable> asyncOperation)
        {
            asyncOperation.Completed -= OnTableFound;
            var entry = asyncOperation.Result.GetEntry(currentText);
            if (entry == currentTableEntry)
                return;
            
            currentTableEntry = entry;
            if (entry == null)
                return;

            hasChanged = true;
            var localizedText = new LocalizedString(localizeEvent.StringReference.TableReference, currentTableEntry.KeyId);
            localizeEvent.StringReference = localizedText;
            localizeEvent.RefreshString();
        }

        private void LateUpdate()
        {
            hasChanged = false;
        }
    }
}
#endif
