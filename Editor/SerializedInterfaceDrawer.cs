using UnityEditor;
using UnityEngine;

namespace Bipolar.Editor
{
    [CustomPropertyDrawer(typeof(Serialized<>))]
    public class SerializedInterfaceDrawer : PropertyDrawer
    {
        private const string errorMessage = "Provided type is not an interface";

        private const string serializedObjectPropertyName = "serializedObject";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var objectFieldRect = new Rect(position.x, position.y, position.width - InterfaceSelectorButton.Width, position.height);
            var interfaceButtonRect = new Rect(position.x + objectFieldRect.width, position.y, InterfaceSelectorButton.Width, position.height);

            var serializedObjectProperty = property.FindPropertyRelative(serializedObjectPropertyName);

            var requiredType = fieldInfo.FieldType.GetGenericArguments()[0];
            serializedObjectProperty.objectReferenceValue = EditorGUI.ObjectField(objectFieldRect, label, serializedObjectProperty.objectReferenceValue, requiredType, true);
            if (GUI.Button(interfaceButtonRect, "I"))
            {
                Object objectReferenceValue = serializedObjectProperty.objectReferenceValue;
                InterfaceSelectorWindow.Show(requiredType, objectReferenceValue, (obj) => AssignValue(serializedObjectProperty, obj));
            }

            EditorGUI.EndProperty();
        }

        private static void AssignValue(SerializedProperty property, Object @object)
        {
            property.objectReferenceValue = @object;
            property.serializedObject.ApplyModifiedProperties();
        }
    }

    public static class InterfaceSelectorButton
    {
        public const float Width = 20;
    }
}
