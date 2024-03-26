using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Bipolar.Editor
{
    [CustomPropertyDrawer(typeof(SerializedInterface<>))]
    public class SerializedInterfaceDrawer : PropertyDrawer
    {
        private const string serializedObjectPropertyName = "serializedObject";
        private const BindingFlags typePropertyBindings = BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.NonPublic;
        
        private readonly PropertyInfo typePropertyInfo = typeof(SerializedInterface<>).GetProperty("Type", typePropertyBindings); 

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            var serializedObjectProperty = property.FindPropertyRelative(serializedObjectPropertyName);

            var requiredType = fieldInfo.FieldType.GetGenericArguments()[0];
            serializedObjectProperty.objectReferenceValue = EditorGUI.ObjectField(position, label, serializedObjectProperty.objectReferenceValue, requiredType, true);
            
            EditorGUI.EndProperty();
        }
    }
}
