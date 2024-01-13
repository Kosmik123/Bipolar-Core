using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Rendering;

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
            public int itemIndex;
            public Object[] assetsOfType;

            public InterfacePickerWindowData(System.Type interfaceType)
            {
                filteredType = interfaceType;
                assetsOfType = GetAssetsOfType(interfaceType);
            }
        }

        #region Constants
        private const string searchBoxName = "searchBox";
        private static readonly string[] tabs = { "Assets", "Scene" };
        private static readonly GUILayoutOption[] tabsLayout = { GUILayout.MaxWidth(110) };
        private static GUIStyle _selectedStyle;
        private static GUIStyle SelectedStyle
        {
            get
            {
                if (_selectedStyle == null)
                {
                    var backgroundTexture = new Texture2D(1, 1);
                    backgroundTexture.SetPixel(0, 0, new Color32(62, 95, 150, 255));
                    backgroundTexture.Apply();

                    _selectedStyle = new GUIStyle(EditorStyles.label);
                    _selectedStyle.normal.textColor = Color.white;
                    _selectedStyle.normal.background = backgroundTexture;
                }
                return _selectedStyle;
            }
        }
        #endregion

        private readonly static Dictionary<System.Type, InterfacePickerWindowData> windowsByType = new Dictionary<System.Type, InterfacePickerWindowData>();

        private InterfacePickerWindowData data;
        private float assetsViewScrollAmount;

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

            switch (data.tab)
            {
                case 0:
                    LoadAssetsPanel();
                    break;
                case 1:
                    break;
            }
        }

        private void LoadAssetsPanel()
        {
            assetsViewScrollAmount = EditorGUILayout.BeginScrollView(new Vector2(0, assetsViewScrollAmount)).y;

            EditorGUIUtility.SetIconSize(new Vector2(16, 16));
            int pressedAssetIndex = data.itemIndex;
            if (DrawAssetListItem(0, "None"))
                pressedAssetIndex = 0;
            for (int i = 0; i < data.assetsOfType.Length; i++)
            {
                Object asset = data.assetsOfType[i];
                var image = AssetPreview.GetMiniThumbnail(asset);
                if (DrawAssetListItem(i + 1, asset.name, image))
                    pressedAssetIndex = i + 1;
            }
            data.itemIndex = pressedAssetIndex;
            EditorGUIUtility.SetIconSize(Vector2.zero);

            EditorGUILayout.EndScrollView();
            
            //string folderName = "Assets/Scripts";
            //string[] filesNames = Directory.GetFiles(folderName);
            //for (int i = 0; i < filesNames.Length; i++)
            //{
            //    filesNames[i] = filesNames[i].Replace('\\', '/');
            //    var asset = AssetDatabase.LoadAssetAtPath<Object>(filesNames[i]);
            //    var image = AssetPreview.GetMiniThumbnail(asset);
            //    GUILayout.Label(new GUIContent(filesNames[i], image));    
            //}

            //GUILayout.BeginHorizontal();
            //data.itemIndex = EditorGUILayout.Popup(data.itemIndex, filesNames, GUILayout.Width(100));
            //GUILayout.EndHorizontal();
        }

        private bool DrawAssetListItem(int index, string assetName, Texture2D assetImage = null)
        {
            bool wasPressed = false;
            GUILayout.BeginHorizontal();
            GUILayout.Space(assetImage ? 20 : 36);
            var style = index == data.itemIndex ? SelectedStyle : EditorStyles.label;
            if (GUILayout.Button(new GUIContent(assetName, assetImage), style))
                wasPressed = true;
            GUILayout.EndHorizontal();
            return wasPressed;
        }

        /// <summary>
        /// Used to get assets of a certain type and file extension from entire project
        /// </summary>
        /// <param name="type">The type to retrieve. eg typeof(GameObject).</param>
        /// <param name="fileExtension">The file extention the type uses eg ".prefab".</param>
        /// <returns>An Object array of assets.</returns>
        public static Object[] GetAssetsOfType(System.Type type, string fileExtension = ".asset")
        {
            var foundObjectsList = new List<Object>();
            var directory = new DirectoryInfo(Application.dataPath);
            var files = directory.GetFiles("*" + fileExtension, SearchOption.AllDirectories);

            foreach (var fileInfo in files)
            {
                if (fileInfo == null)
                    continue;

                var assetFilePath = fileInfo.FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets");
                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetFilePath);
                if (asset == null)
                {
                    continue;
                }
                else if (type.IsAssignableFrom(asset.GetType()) == false)
                {
                    continue;
                }

                foundObjectsList.Add(asset);
            }

            return foundObjectsList.ToArray();
        }






        private void OnLostFocus()
        {
            Close();
        }
    }
}
