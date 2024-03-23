using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;

namespace Bipolar.Editor
{
    [CustomPropertyDrawer(typeof(RequireInterfaceAttribute))]
    public class RequireInterfaceDrawer : PropertyDrawer
    {
        private const string errorMessage = "Property is not a reference type";
        private const int interfaceButtonWidth = 20;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }

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
                    var window = InterfacePickerWindow.Show(requiredAttribute.RequiredType, property.objectReferenceValue);
                    window.OnClosed += (obj) => AssignValue(property, obj);
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

        private void AssignValue (SerializedProperty property, Object @object)
        {
            property.objectReferenceValue = @object;
            property.serializedObject.ApplyModifiedProperties();
        }
    }

    public class InterfacePickerWindow : EditorWindow
    {
        private class InterfacePickerWindowData
        {
            public System.Type filteredType;
            public bool isFocused = false;
            public int tab;
            public ScriptableObject[] assetsOfType;

            public InterfacePickerWindowData(System.Type interfaceType)
            {
                filteredType = interfaceType;
                assetsOfType = GetAssetsOfType(interfaceType);
            }
        }

        public event System.Action<Object> OnClosed;

        #region Constants
        private const string noneObjectName = "None";
        private const string searchBoxName = "searchBox";
        private static readonly string[] tabs = { "Assets", "Scene" };
        private static readonly GUILayoutOption[] tabsLayout = { GUILayout.MaxWidth(110) };
        private static GUIStyle _selectedStyle;
        private static GUIStyle SelectedStyle
        {
            get
            {
                if (_selectedStyle == null || _selectedStyle.normal.background == null)
                {
                    var backgroundTexture = new Texture2D(1, 1);
                    backgroundTexture.SetPixel(0, 0, new Color32(62, 95, 150, 255));
                    backgroundTexture.Apply();

                    _selectedStyle = new GUIStyle(EditorStyles.label);
                    _selectedStyle.name = "Selected";
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
        private float sceneObjectsViewScrollAmount;
        private Object selectedObject;
        private string searchFilter = "";
        private GameObject[] gameObjectsWithCorrectComponent;

        public static InterfacePickerWindow Show(System.Type interfaceType, Object selectedObject)
        {
            var window = Get(interfaceType);
            window.selectedObject = selectedObject;
            window.ShowUtility();
            return window;
        }

        private static InterfacePickerWindow Get(System.Type interfaceType)
        {
            var window = CreateInstance<InterfacePickerWindow>();
            window.titleContent = new GUIContent($"Select {interfaceType.Name}");
            window.data = GetData(interfaceType);
            window.gameObjectsWithCorrectComponent = GetGameObjects(interfaceType);
            return window;
        }

        private static InterfacePickerWindowData GetData(System.Type interfaceType)
        {
            if (windowsByType.TryGetValue(interfaceType, out var existingData))
            {
                if (existingData != null)
                {
                    existingData.isFocused = false;
                    return existingData; ;
                }
            }

            var newData = new InterfacePickerWindowData(interfaceType);
            windowsByType[interfaceType] = newData;
            return newData;
        }

        private static GameObject[] GetGameObjects(System.Type interfaceType)
        {
            var gameObjectsWithComponent = new List<GameObject>();
            var allGameObjects = FindObjectsOfType<GameObject>(true);
            foreach (var gameObject in allGameObjects)
                if (gameObject.TryGetComponent(interfaceType, out _))
                    gameObjectsWithComponent.Add(gameObject);

            return gameObjectsWithComponent.ToArray();
        }

        private void OnGUI()
        {
            GUI.SetNextControlName(searchBoxName);
            searchFilter = EditorGUILayout.TextField(searchFilter, EditorStyles.toolbarSearchField);
            if (data.isFocused == false)
            {
                GUI.FocusControl(searchBoxName);
                data.isFocused = true;
            }

            data.tab = GUILayout.Toolbar(data.tab, tabs, tabsLayout);

            switch (data.tab)
            {
                case 0:
                    DrawAssetsPanel();
                    break;
                case 1:
                    DrawSceneObjectsPanel();
                    break;
            }
        }

        private void DrawSceneObjectsPanel()
        {
            sceneObjectsViewScrollAmount = EditorGUILayout.BeginScrollView(new Vector2(0, sceneObjectsViewScrollAmount)).y;
            EditorGUIUtility.SetIconSize(new Vector2(16, 16));
            Object pressedObject = selectedObject;
            if (DrawGameObjectListItem(null))
            {
                pressedObject = null;
            }
            foreach (var gameObject in gameObjectsWithCorrectComponent)
            {
                if (gameObject.name.ToLower().Contains(searchFilter.ToLower()))
                {
                    if (DrawGameObjectListItem(gameObject))
                    {
                        pressedObject = gameObject.GetComponent(data.filteredType);
                    }
                }
            }

            EditorGUIUtility.SetIconSize(Vector2.zero);
            EditorGUILayout.EndScrollView();

            if (selectedObject == pressedObject && Event.current.clickCount > 1)
            {
                Event.current.clickCount = 0;
                Close();
            }

            selectedObject = pressedObject;
        }
        
        private bool DrawGameObjectListItem(GameObject gameObject)
        {
            bool wasPressed = false;
            GUILayout.BeginHorizontal();
            var objectContent = EditorGUIUtility.ObjectContent(gameObject, typeof(GameObject));
            objectContent.text = gameObject ? gameObject.name : noneObjectName;
            var style = EditorStyles.label;
            if (selectedObject is Component component && component.gameObject == gameObject || selectedObject == gameObject)
                style = SelectedStyle;

            GUILayout.Space(20);
            if (GUILayout.Button(objectContent, style))
                wasPressed = true;
            GUILayout.EndHorizontal();
            return wasPressed;
        }

        private void DrawAssetsPanel()
        {
            assetsViewScrollAmount = EditorGUILayout.BeginScrollView(new Vector2(0, assetsViewScrollAmount)).y;

            EditorGUIUtility.SetIconSize(new Vector2(16, 16));
            Object pressedObject = selectedObject;
            if (DrawAssetListItem(null))
            {
                pressedObject = null;
            }
            foreach (var asset in data.assetsOfType)
            {
                if (asset.name.ToLower().Contains(searchFilter.ToLower()))
                {
                    if (DrawAssetListItem(asset))
                    {
                        pressedObject = asset;
                    } 
                }
            }
            EditorGUIUtility.SetIconSize(Vector2.zero);
            EditorGUILayout.EndScrollView();

            if (selectedObject == pressedObject && Event.current.clickCount > 1)
            {
                Event.current.clickCount = 0;
                Close();
            }

            selectedObject = pressedObject;
        }

        private bool DrawAssetListItem(ScriptableObject asset)
        {
            bool wasPressed = false;
            GUILayout.BeginHorizontal();

            var image = AssetPreview.GetMiniThumbnail(asset);
            GUILayout.Space(image ? 20 : 36);
            string name = asset ? asset.name : noneObjectName;
            var style = asset == selectedObject ? SelectedStyle : EditorStyles.label;
            if (GUILayout.Button(new GUIContent(name, image), style))
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
        public static ScriptableObject[] GetAssetsOfType(System.Type type, string fileExtension = "asset")
        {
            var derivedTypes = TypeCache.GetTypesDerivedFrom(type);
            var filterBuilder = new StringBuilder();
            foreach (var derivedType in derivedTypes)
            {
                if (derivedType.IsSubclassOf(typeof(ScriptableObject)))
                    filterBuilder.Append($"t:{derivedType.Name} ");
            }
            if (filterBuilder.Length < 1)
                filterBuilder.Append("t:");

            var foundObjectsList = new List<ScriptableObject>();

            var assetsGuids = AssetDatabase.FindAssets(filterBuilder.ToString());
            foreach (var assetGuid in assetsGuids)
            {
                var assetFilePath = AssetDatabase.GUIDToAssetPath(assetGuid);
                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetFilePath);
                if (asset == null)
                    continue;
                else if (type.IsAssignableFrom(asset.GetType()) == false)
                    continue;

                foundObjectsList.Add(asset);
            }
            return foundObjectsList.ToArray();
        }

        private void OnLostFocus()
        {
            Close();
        }

        private void OnDestroy()
        {
            OnClosed?.Invoke(selectedObject);
            OnClosed = null;
        }
    }
}
