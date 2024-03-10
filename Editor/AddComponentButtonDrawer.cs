using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bipolar
{
    [CustomPropertyDrawer(typeof(AddComponentButtonAttribute))]
    public class AddComponentButtonDrawer : PropertyDrawer
    {
        public const int buttonDistance = 4;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
#if UNITY_SRP
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                //using (var horizontalScope = new EditorGUILayout.HorizontalScope())
                {
                    var buttonAttribute = attribute as AddComponentButtonAttribute;
                    float buttonWidth = buttonAttribute.Width;
                    position = new Rect(position.position, new Vector2(position.width - buttonWidth - buttonDistance, position.height));
                    var buttonRect = new Rect(position.xMax + buttonDistance, position.y, buttonWidth, position.height);
                    if (GUI.Button(buttonRect, "Add", EditorStyles.miniButton))
                    {
                        // TODO add component dropdown
                        //UnityEditor.Rendering.FilterWindow.Show(position.center, new ComponentProvider(fieldInfo.FieldType));
                    }
                }
            }
#endif

            EditorGUI.PropertyField(position, property, label);
            EditorGUI.EndProperty();
        }
    }

#if UNITY_SRP
    public class ComponentProvider : UnityEditor.Rendering.FilterWindow.IProvider
    {
        public System.Type ComponentType { get; protected set; }

        public ComponentProvider(System.Type type)
        {
            ComponentType = type;
        }

        public Vector2 position { get; set; }

        public void CreateComponentTree(List<UnityEditor.Rendering.FilterWindow.Element> tree)
        {
        }

        public bool GoToChild(UnityEditor.Rendering.FilterWindow.Element element, bool addIfComponent)
        {
           return false;
        }
    }
#endif

}
