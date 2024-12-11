#define VOLUME

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Bipolar.Subcomponents.Editor
{
	[CustomEditor(typeof(CompoundBehavior<>), editorForChildClasses: true)]
	public class CompoundBehaviorEditor : UnityEditor.Editor
	{
		private SerializedProperty componentsListProperty;
		private ICompoundBehavior compoundBehavior;

		private static readonly GUIContent buttonContent = new GUIContent($"Add Subcomponent");

		private void OnEnable()
		{
			componentsListProperty = serializedObject.FindProperty("subcomponents");
			compoundBehavior = target as ICompoundBehavior;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (componentsListProperty != null)
			{
				GUILayout.Space(10);
				bool changed = false;

				int count = compoundBehavior.SubcomponentsCount;

				EditorGUI.BeginChangeCheck();
				for (int i = 0; i < count; i++)
				{
					EditorUtility.DrawSplitter();
					var subcomponent = compoundBehavior.Subcomponents[i];
					var itemProperty = componentsListProperty.GetArrayElementAtIndex(i);
					DrawSubcomponent(itemProperty, subcomponent);
				}
				changed |= EditorGUI.EndChangeCheck();


				EditorUtility.DrawSplitter();
				GUILayout.Space(6);

				var buttonRect = EditorGUILayout.GetControlRect(); 
				if (count >= 0 && EditorGUI.DropdownButton(buttonRect, buttonContent, FocusType.Keyboard, EditorStyles.miniButton))
				{
					var popupRect = buttonRect;
					var center = popupRect.center;
					popupRect.width = 250;
					popupRect.center = center;
					//componentsListProperty.InsertArrayElementAtIndex(count);
					//var createdItemProperty = componentsListProperty.GetArrayElementAtIndex(count);
					//AddSubcomponentWindow.Show(compoundBehavior.SubcomponentsType, buttonRect);
					var popup = AddSubcomponentPopup.Get(compoundBehavior.SubcomponentsType);
					popup.Show(popupRect);
					changed = true;
				}
				GUILayout.Space(6);

				if (changed)
					serializedObject.ApplyModifiedProperties();
			}
		}

		public static void DrawSubcomponent(SerializedProperty property, ISubcomponent subcomponent)
		{
#if VOLUME9
			const float height = 17f;
			var backgroundRect = GUILayoutUtility.GetRect(1f, height);

			var labelRect = backgroundRect;
			labelRect.xMin += 32f;
			labelRect.xMax -= 20f + 16 + 5;

			var foldoutRect = backgroundRect;
			foldoutRect.y += 1f;
			foldoutRect.width = 13f;
			foldoutRect.height = 13f;

			var toggleRect = backgroundRect;
			toggleRect.x += 16f;
			toggleRect.y += 2f;
			toggleRect.width = 13f;
			toggleRect.height = 13f;

			// Background rect should be full-width
			backgroundRect.xMin = 0f;
			backgroundRect.width += 4f;

			// Background
			float backgroundTint = EditorGUIUtility.isProSkin ? 0.1f : 1f;
			EditorGUI.DrawRect(backgroundRect, new Color(backgroundTint, backgroundTint, backgroundTint, 0.2f));

			// Title
			using (new EditorGUI.DisabledScope(!activeField.boolValue))
				EditorGUI.LabelField(labelRect, title, EditorStyles.boldLabel);

			// Foldout
			group.serializedObject.Update();
			group.isExpanded = GUI.Toggle(foldoutRect, group.isExpanded, GUIContent.none, EditorStyles.foldout);
			group.serializedObject.ApplyModifiedProperties();

			// Active checkbox
			activeField.serializedObject.Update();
			activeField.boolValue = GUI.Toggle(toggleRect, activeField.boolValue, GUIContent.none, CoreEditorStyles.smallTickbox);
			activeField.serializedObject.ApplyModifiedProperties();


			// Context menu
			Texture menuIcon = null;
			var menuRect = new Rect(labelRect.xMax + 3f + 16 + 5, labelRect.y + 1f, menuIcon.width, menuIcon.height);

			if (contextAction != null)
				GUI.DrawTexture(menuRect, menuIcon);

			// Documentation button
			if (!String.IsNullOrEmpty(documentationURL))
			{
				var documentationRect = menuRect;
				documentationRect.x -= 16 + 5;
				documentationRect.y -= 1;

				var documentationTooltip = $"Open Reference for {title.text}.";
				var documentationIcon = new GUIContent(EditorGUIUtility.TrIconContent("_Help").image, documentationTooltip);
				var documentationStyle = new GUIStyle("IconButton");

				if (GUI.Button(documentationRect, documentationIcon, documentationStyle))
					System.Diagnostics.Process.Start(documentationURL);
			}

			// Handle events
			var e = Event.current;

			if (e.type == EventType.MouseDown)
			{
				if (contextAction != null && menuRect.Contains(e.mousePosition))
				{
					contextAction(new Vector2(menuRect.x, menuRect.yMax));
					e.Use();
				}
				else if (labelRect.Contains(e.mousePosition))
				{
					if (e.button == 0)
						group.isExpanded = !group.isExpanded;
					else if (contextAction != null)
						contextAction(e.mousePosition);

					e.Use();
				}
			}

#else
			var headerRect = GUILayoutUtility.GetRect(1f, 18f); //EditorGUILayout.GetControlRect();

			var backgroundRect = headerRect;
			backgroundRect.xMin = 0;
			float backgroundTint = EditorGUIUtility.isProSkin ? 0.1f : 1f;
			EditorGUI.DrawRect(backgroundRect, new Color(backgroundTint, backgroundTint, backgroundTint, 0.2f));

			headerRect.y -= 1;
			headerRect.x += 12;
			bool isExpanded = EditorGUI.Foldout(headerRect, property.isExpanded, GUIContent.none);
			property.isExpanded = isExpanded;


			var toggleRect = headerRect;
			toggleRect.x += 4;
			toggleRect.y += 3;
			toggleRect.width = 20;

			if (subcomponent is SubBehavior behavior)
			{
				behavior.IsEnabled = EditorGUI.Toggle(toggleRect, behavior.IsEnabled, new GUIStyle("ShurikenToggle"));
			}

			var labelRect = headerRect;
			labelRect.xMin += 20f;
			labelRect.xMax -= 20f + 16 + 5;
			string typeName = subcomponent.GetType().Name;

			EditorGUI.LabelField(labelRect, ObjectNames.NicifyVariableName(typeName), EditorStyles.boldLabel);

			//var propertyRect = EditorGUILayout.GetControlRect();
			//EditorGUI.PropertyField(propertyRect, property);

			if (isExpanded)
			{
				using (new EditorGUI.IndentLevelScope())
				{

					//		// Check if a custom property drawer exists for this type.
					//		PropertyDrawer customDrawer = GetCustomPropertyDrawer(property);
					//		if (customDrawer != null)
					//		{
					//			// Draw the property with custom property drawer.
					//			Rect indentedRect = position;
					//			float foldoutDifference = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
					//			indentedRect.height = customDrawer.GetPropertyHeight(property, label);
					//			indentedRect.y += foldoutDifference;
					//			customDrawer.OnGUI(indentedRect, property, label);
					//		}
					//		else
					//		{
					//			// Draw the properties of the child elements.
					//			// NOTE: In the following code, since the foldout layout isn't working properly, I'll iterate through the properties of the child elements myself.
					//			// EditorGUI.PropertyField(position, property, GUIContent.none, true);

					//Rect childPosition = position;
					//childPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
					foreach (var childProperty in GetChildProperties(property))
					{
						if (childProperty != null && childProperty.name != "<IsEnabled>k__BackingField")
						{
							EditorGUILayout.PropertyField(childProperty, true);
						}
					}
					//		}


				}
				EditorGUILayout.Space(4);
			}
#endif
		}

		private static string GetPropertyTypeName(SerializedProperty property)
		{
			var typeName = property.type;
			if (property.propertyType != SerializedPropertyType.ManagedReference)
				return typeName;
			
			int typeNameStart = typeName.IndexOf('<') + 1;
			int typeNameEnd = typeName.LastIndexOf('>');
			int typeNameLength = typeNameEnd - typeNameStart;
			if (typeNameLength < 0)
				return string.Empty;

			return typeName.Substring(typeNameStart, typeNameLength);
		}

		public static IEnumerable<SerializedProperty> GetChildProperties(SerializedProperty parent)
		{
			int depthOfParent = parent.depth;
			var iterator = parent.Copy();
			bool searchChildren = true;
			while (iterator.Next(searchChildren))
			{
				searchChildren = false;
				if (iterator.depth <= depthOfParent)
					break;

				yield return iterator.Copy();
			}
		}
		private void OnDisable()
		{
		}
	}

	public static class EditorUtility
	{

		public static void DrawSplitter(bool isBoxed = false)
		{
			var rect = GUILayoutUtility.GetRect(1f, 1f);
			float xMin = rect.xMin;
			rect.x -= 1;
			
			// Splitter rect should be full-width
			rect.xMin = 0f;
			rect.width += 4f;

			if (isBoxed)
			{
				rect.xMin = xMin == 7.0 ? 4.0f : EditorGUIUtility.singleLineHeight;
				rect.width -= 1;
			}

			if (Event.current.type != EventType.Repaint)
				return;

			EditorGUI.DrawRect(rect, EditorGUIUtility.isProSkin
				? new Color(0.12f, 0.12f, 0.12f, 1.333f)
				: new Color(0.6f, 0.6f, 0.6f, 1.333f));
		}
	}

	internal class AddSubcomponentPopup : AdvancedDropdown
	{
		public class Item : AdvancedDropdownItem
		{
			public Type Type { get; }

			public Item(Type type) : this(type, ObjectNames.NicifyVariableName(type.Name)) 
			{ }
			
			public Item(Type type, string name) : base(name)
			{
				Type = type;
			}
		}

		private static readonly Dictionary<Type, AddSubcomponentPopup> cachedPopups = new Dictionary<Type, AddSubcomponentPopup>();

		public Type SubcomponentType { get; private set; }
		public ICollection<Type> Types { get; private set; }

		private AdvancedDropdownItem root;

		public static AddSubcomponentPopup Get(Type subcomponentType)
		{
			if (cachedPopups.TryGetValue(subcomponentType, out var popup) == false)
			{
				popup = new AddSubcomponentPopup(subcomponentType);
				cachedPopups.Add(subcomponentType, popup);
			}
			return popup;
		}

		private AddSubcomponentPopup(Type subcomponentType) : base (new AdvancedDropdownState())
		{
			SubcomponentType = subcomponentType;
			Types = TypeCache.GetTypesDerivedFrom(SubcomponentType);
			
			root = new AdvancedDropdownItem("Subcomponent");
			foreach (var type in Types)
			{
				if (type.IsDefined(typeof(AddComponentMenu), true)) 
				{
					var attribute = (AddComponentMenu)type.GetCustomAttributes(typeof(AddComponentMenu), true)[0];
					var path = attribute.componentMenu;
					string[] pathItems = path.Contains('/') 
						? path.Split('/')
						: path.Split('\\');

					var subcomponentName = pathItems[pathItems.Length - 1];
					if (string.IsNullOrWhiteSpace(subcomponentName) == false)
					{
						var node = root;
						for (int i = 0; i < pathItems.Length - 1; i++)
						{
							node = node.GetOrCreateChild(pathItems[i]);
						}
						node.AddChild(new Item(type, subcomponentName));
					}
					continue;
				}

				root.AddChild(new Item(type));
			}
		}

		protected override AdvancedDropdownItem BuildRoot() => root;
	}

	//internal class AddSubcomponentWindowContent : PopupWindowContent
	internal class AddSubcomponentWindow : EditorWindow
	{
		public Type Type { get; private set; }

		public static AddSubcomponentWindow Show(Type subcomponentType, Rect buttonRect)
		{
			//PopupWindow.Show(new Rect(0, 0, 200, 300), new AddSubcomponentWindow());

			var window = GetWindow<AddSubcomponentWindow>();

			window.Type = subcomponentType;
			//window.ShowPopup(); // buttonRect, new Vector2(200, 300));
			window.titleContent = null;
			//window.ShowAuxWindow();
			window.ShowModalUtility();
			window.Focus();

			//window.titleContent = null;//	 new GUIContent(subcomponentType.Name);
			//return null;
			return window;
		}

		private void OnLostFocus()
		{
			Close();
		}

		//public override void OnGUI(Rect rect)
		//{
		//	EditorGUI.LabelField(new Rect(), Type.Name);
		//}
	}

	public static class AdvancedDropdownItemExtension
	{
		public static AdvancedDropdownItem WithChildrenAdded(this AdvancedDropdownItem parent, IEnumerable<AdvancedDropdownItem> children)
		{
			foreach (var child in children)
				parent.AddChild(child);
			return parent;
		}

		public static AdvancedDropdownItem GetOrCreateChild(this AdvancedDropdownItem parent, string childName)
		{
			var child = parent.children.FirstOrDefault(ch => ch.name == childName);
			if (child == null)
			{
				child = new AdvancedDropdownItem(childName);
				parent.AddChild(child);
			}

			return child;
		}
	}
}
