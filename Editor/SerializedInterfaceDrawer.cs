using UnityEditor;
using UnityEngine;

namespace Bipolar.Editor
{
    [CustomPropertyDrawer(typeof(Serialized<>), true)]
    [CustomPropertyDrawer(typeof(Serialized<,>), true)]
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

            var requiredType = default(System.Type);  
            for (var type = fieldInfo.FieldType; type != null; type = type.BaseType)
            {
                if (type.IsGenericType == false)
                    continue;

                requiredType = type.GetGenericArguments()[0];
                break;
            }

            if (requiredType != default)
            {
                serializedObjectProperty.objectReferenceValue = EditorGUI.ObjectField(objectFieldRect, label, serializedObjectProperty.objectReferenceValue, requiredType, true);
                if (GUI.Button(interfaceButtonRect, "I"))
                {
                    Object objectReferenceValue = serializedObjectProperty.objectReferenceValue;
                    InterfaceSelectorWindow.Show(requiredType, objectReferenceValue, (obj) => AssignValue(serializedObjectProperty, obj));
                }
            }
            else
            {

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
