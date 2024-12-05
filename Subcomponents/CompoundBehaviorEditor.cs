#define VOLUME

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bipolar.Subcomponents.Editor
{
	[CustomEditor(typeof(CompoundBehavior<>), editorForChildClasses: true)]
	public class CompoundBehaviorEditor : UnityEditor.Editor
	{
		private SerializedProperty componentsListProperty;
		private ICompoundBehavior compoundBehavior;

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

				if (count >= 0 && GUILayout.Button($"Add Component ({count})", EditorStyles.miniButton))
				{
					componentsListProperty.InsertArrayElementAtIndex(count);
					var createdItemProperty = componentsListProperty.GetArrayElementAtIndex(count);
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
			var headerRect = EditorGUILayout.GetControlRect();
			
			float backgroundTint = EditorGUIUtility.isProSkin ? 0.1f : 1f;
			EditorGUI.DrawRect(headerRect, new Color(backgroundTint, backgroundTint, backgroundTint, 0.2f));


			headerRect.x += 12;
			bool isExpanded = EditorGUI.Foldout(headerRect, property.isExpanded, GUIContent.none);
			property.isExpanded = isExpanded;


			var toggleRect = headerRect;
			toggleRect.x += 4;
			toggleRect.y += 3;
			toggleRect.width = 20;

			if (subcomponent is ISubBehavior behavior) 
			{
				behavior.IsEnabled = EditorGUI.Toggle(toggleRect, behavior.IsEnabled, new GUIStyle("ShurikenToggle"));
			}


			var labelRect = headerRect;
			labelRect.xMin += 20f;
			labelRect.xMax -= 20f + 16 + 5;
			string displayedName = ObjectNames.NicifyVariableName(property.type);
			EditorGUI.LabelField(labelRect, displayedName, EditorStyles.boldLabel);


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
					foreach (SerializedProperty childProperty in property.GetChildProperties())
					{
						if (childProperty != null)
							EditorGUILayout.PropertyField(childProperty, true);

					}
					//		}


				}
				EditorGUILayout.Space(4);
			}




#endif
		}

		private void OnDisable()
		{
		}
	}

	public static class SerializedPropertyExtensions
	{
		public static IEnumerable<SerializedProperty> GetChildProperties(this SerializedProperty parent)
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

	}


	public static class EditorUtility
	{
		public static void DrawSplitter(bool isBoxed = false)
		{
			var rect = GUILayoutUtility.GetRect(1f, 1f);
			float xMin = rect.xMin;

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

			EditorGUI.DrawRect(rect, !EditorGUIUtility.isProSkin
				? new Color(0.6f, 0.6f, 0.6f, 1.333f)
				: new Color(0.12f, 0.12f, 0.12f, 1.333f));
		}

	}
}
