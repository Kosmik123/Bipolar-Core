using System;
using UnityEditor;
using UnityEngine;

namespace Bipolar.Editor
{
    [CustomPropertyDrawer(typeof(SerializedGuid))]
    public class SerializedGuidDrawer : PropertyDrawer
    {
        private const float buttonWidth = 65;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var aProperty = property.FindPropertyRelative("a");
            var bProperty = property.FindPropertyRelative("b");
            var cProperty = property.FindPropertyRelative("c");
            var dProperty = property.FindPropertyRelative("d");
            var eProperty = property.FindPropertyRelative("e");
            var fProperty = property.FindPropertyRelative("f");
            var gProperty = property.FindPropertyRelative("g");
            var hProperty = property.FindPropertyRelative("h");
            var iProperty = property.FindPropertyRelative("i");
            var jProperty = property.FindPropertyRelative("j");
            var kProperty = property.FindPropertyRelative("k");
            var backingGuid = new Guid(aProperty.intValue,
                (short)bProperty.intValue,
                (short)cProperty.intValue,
                (byte)dProperty.intValue, 
                (byte)eProperty.intValue, 
                (byte)fProperty.intValue, 
                (byte)gProperty.intValue, 
                (byte)hProperty.intValue, 
                (byte)iProperty.intValue, 
                (byte)jProperty.intValue, 
                (byte)kProperty.intValue);

            var buttonRect = position;
            bool isSmall = position.width - EditorGUIUtility.labelWidth < 2 * buttonWidth;
            buttonRect.width = isSmall ? 0 : buttonWidth;

            var textRect = position;
            textRect.width -= buttonRect.width;
            buttonRect.x += textRect.width + 1;

            var labelRect = textRect;
            labelRect.width = EditorGUIUtility.labelWidth - EditorGUI.indentLevel * 15;
            
            textRect.x += labelRect.width + 2;
            textRect.width -= labelRect.width + 2;

            bool isPrefabOverride = property.prefabOverride;
            EditorGUI.LabelField(labelRect, label, isPrefabOverride ? EditorStyles.boldLabel : EditorStyles.label);
            var font = GUI.skin.font;
            if (isPrefabOverride) 
                GUI.skin.font = EditorStyles.boldFont;
            EditorGUI.TextField(textRect, backingGuid.ToString());
            GUI.skin.font = font;

            if (GUI.Button(buttonRect, "New"))
            {
                property.serializedObject.Update();
                var newGuid = Guid.NewGuid();
                newGuid.GetBackingValues(out var a, out var b, out var c, out var d, out var e,
                    out var f, out var g, out var h, out var i, out var j, out var k);

                aProperty.intValue = a;
                bProperty.intValue = b;
                cProperty.intValue = c;
                dProperty.intValue = d;
                eProperty.intValue = e;
                fProperty.intValue = f;
                gProperty.intValue = g;
                hProperty.intValue = h;
                iProperty.intValue = i;
                jProperty.intValue = j;
                kProperty.intValue = k;

                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}