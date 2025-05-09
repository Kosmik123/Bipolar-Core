using UnityEditor;
using UnityEngine;

namespace Bipolar.Editor
{
    [CustomPropertyDrawer(typeof(Optional<>), true)]
    public class OptionalPropertyDrawer : PropertyDrawer
    {
        private static readonly GUIContent noLabel = GUIContent.none;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var hasValueProperty = property.FindPropertyRelative("hasValue");
            bool hasValue = hasValueProperty.boolValue;
            if (hasValue == false)
                return base.GetPropertyHeight(property, label);

            var valueProperty = property.FindPropertyRelative("value");
            return EditorGUI.GetPropertyHeight(valueProperty);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var checkboxRect = position;
            checkboxRect.width = 20;
            checkboxRect.height = 20;
            var hasValueProperty = property.FindPropertyRelative("hasValue");
            EditorGUI.PropertyField(checkboxRect, hasValueProperty, label);

            if (hasValueProperty.boolValue)
            {
                var valueProperty = property.FindPropertyRelative("value");
                var propertyRect = position;
                float propertyOffset = valueProperty.hasVisibleChildren ?
                    checkboxRect.width + EditorGUIUtility.labelWidth + 12 :
                    checkboxRect.width;
                propertyRect.width -= propertyOffset;
                propertyRect.x += propertyOffset;
                EditorGUI.PropertyField(propertyRect, valueProperty, new GUIContent(" "));
            }
        }
    }
}
