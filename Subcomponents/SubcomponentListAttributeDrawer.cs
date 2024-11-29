using UnityEditor;
using UnityEngine;

namespace Bipolar.Subcomponents.Editor
{
	//[CustomPropertyDrawer(typeof(SubcomponentList<>), useForChildren: true)]
	public class SubcomponentListDrawer : PropertyDrawer
	{
		//public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 
		//	SubcomponentListEditor.GetPropertyHeight(property.FindPropertyRelative("items"));

		//public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		//{
		//	EditorGUI.BeginProperty(position, label, property);
		//	var owner = property.serializedObject;
		//	var listProperty = property.FindPropertyRelative("items");
  //          SubcomponentListEditor.Draw(listProperty, owner);
		//	EditorGUI.EndProperty();
		//}
	}
}
