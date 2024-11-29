#define VOLUME

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

				for (int i = 0; i < count; i++)
				{
					var subcomponent = compoundBehavior.Subcomponents[i];
					var itemProperty = componentsListProperty.GetArrayElementAtIndex(i);
					DrawSubcomponent(itemProperty, subcomponent);
				}

				GUILayout.Space(10);
				if (count >= 0 && GUILayout.Button($"Add Component ({count})", EditorStyles.miniButton))
				{
					componentsListProperty.InsertArrayElementAtIndex(count);
					var createdItemProperty = componentsListProperty.GetArrayElementAtIndex(count);
					changed = true;
				}

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
			var backgroundRect = headerRect;

			float backgroundTint = EditorGUIUtility.isProSkin ? 0.1f : 1f;
			EditorGUI.DrawRect(headerRect, new Color(backgroundTint, backgroundTint, backgroundTint, 0.2f));

			string displayedName = ObjectNames.NicifyVariableName(property.type);

			headerRect.x += 12;
			bool isExpanded = EditorGUI.Foldout(headerRect, property.isExpanded, GUIContent.none);

			//var enabledProperty = subcomponentProperty

			var toggleRect = headerRect;
			toggleRect.x += 4;
			toggleRect.y += 3;
			toggleRect.width = 20;

			if (subcomponent is ISubBehavior behavior) 
			{
				behavior.IsEnabled = EditorGUI.Toggle(toggleRect, behavior.IsEnabled, new GUIStyle("ShurikenToggle"));
			}

			property.isExpanded = isExpanded;
#endif	
		}

		private void OnDisable()
		{
		}
	}
}
