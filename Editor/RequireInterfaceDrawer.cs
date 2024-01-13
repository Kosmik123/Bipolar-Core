using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.UIElements;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Bipolar.Editor
{
    [CustomPropertyDrawer(typeof(RequireInterfaceAttribute))]
    public class RequireInterfaceDrawer : PropertyDrawer
    {
        private const string errorMessage = "Property is not a reference type";
        private const int interfaceButtonWidth = 20;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                var objectFieldRect = new Rect(position.x, position.y, position.width - interfaceButtonWidth, position.height);
                var interfaceButtonRect = new Rect(position.x + objectFieldRect.width, position.y, interfaceButtonWidth, position.height);
                
                var requiredAttribute = attribute as RequireInterfaceAttribute;
                EditorGUI.BeginProperty(position, label, property);
                property.objectReferenceValue = EditorGUI.ObjectField(objectFieldRect, label, property.objectReferenceValue, requiredAttribute.RequiredType, true);
                if (GUI.Button(interfaceButtonRect, "I"))
                {
                    InterfacePickerWindow.Show(requiredAttribute.RequiredType);
                }

                EditorGUI.EndProperty();
            }
            else
            {
                var previousColor = GUI.color;
                GUI.color = Color.red;
                EditorGUI.LabelField(position, label, new GUIContent(errorMessage));
                GUI.color = previousColor;
            }
        }
    }

    public class InterfacePickerWindow : EditorWindow
    {
        private class InterfacePickerWindowData
        {
            public System.Type filteredType;
            public bool isFocused = false;
            public int tab;
            
            public InterfacePickerWindowData(System.Type interfaceType)
            {
                filteredType = interfaceType;
            }
        }

        private readonly static Dictionary<System.Type, InterfacePickerWindowData> windowsByType = new Dictionary<System.Type, InterfacePickerWindowData>();
        
        private const string searchBoxName = "searchBox";
        private static readonly string[] tabs = { "Assets", "Scene" };
        private static readonly GUILayoutOption[] tabsLayout = { GUILayout.MaxWidth(110) };

        private InterfacePickerWindowData data;

        public static InterfacePickerWindow Show(System.Type interfaceType)
        {
            var window = Get(interfaceType);
            window.ShowUtility();
            return window;
        }

        private static InterfacePickerWindow Get(System.Type interfaceType)
        {
            var window = CreateInstance<InterfacePickerWindow>();
            window.titleContent = new GUIContent($"Select {interfaceType.Name}");
            window.data = GetData(interfaceType);
            return window;
        }

        private static InterfacePickerWindowData GetData(System.Type interfaceType)
        {
            if (windowsByType.TryGetValue(interfaceType, out var existingData))
            {
                if (existingData != null)
                {
                    existingData.isFocused = false;
                    return existingData;
                }
            }

            var newData = new InterfacePickerWindowData(interfaceType);
            windowsByType[interfaceType] = newData;
            return newData;
        }

        private void OnGUI()
        {
            GUI.SetNextControlName(searchBoxName);
            string filter = EditorGUILayout.TextField(string.Empty, EditorStyles.toolbarSearchField);
            if (data.isFocused == false)
            {
                GUI.FocusControl(searchBoxName);
                data.isFocused = true;
            }

            data.tab = GUILayout.Toolbar(data.tab, tabs, tabsLayout);

        }

        private void LoadPanel(string folderName)
        { }








        private void OnLostFocus()
        {
            Close();
        }
    }
}
