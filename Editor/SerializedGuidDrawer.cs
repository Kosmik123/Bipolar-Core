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
            var a = property.FindPropertyRelative("a");
            var b = property.FindPropertyRelative("b");
            var c = property.FindPropertyRelative("c");
            var d = property.FindPropertyRelative("d");
            var e = property.FindPropertyRelative("e");
            var f = property.FindPropertyRelative("f");
            var g = property.FindPropertyRelative("g");
            var h = property.FindPropertyRelative("h");
            var i = property.FindPropertyRelative("i");
            var j = property.FindPropertyRelative("j");
            var k = property.FindPropertyRelative("k");
            var backingGuid = new Guid(a.intValue,
                (short)b.intValue,
                (short)c.intValue,
                (byte)d.intValue, 
                (byte)e.intValue, 
                (byte)f.intValue, 
                (byte)g.intValue, 
                (byte)h.intValue, 
                (byte)i.intValue, 
                (byte)j.intValue, 
                (byte)k.intValue);

            var buttonRect = position;
            bool isSmall = position.width - EditorGUIUtility.labelWidth < 2 * buttonWidth;
            buttonRect.width = isSmall ? 0 : buttonWidth;

            var textRect = position;
            textRect.width -= buttonRect.width;
            buttonRect.x += textRect.width + 1;

            var labelRect = textRect;
            labelRect.width = EditorGUIUtility.labelWidth;
            
            textRect.x += labelRect.width + 2;
            textRect.width -= labelRect.width + 2;

            bool isPrefabOverride = property.prefabOverride;
            EditorGUI.LabelField(labelRect, label, isPrefabOverride ? EditorStyles.boldLabel : EditorStyles.label);
            EditorGUI.TextField(textRect, backingGuid.ToString());

            if (GUI.Button(buttonRect, "New"))
            {
                property.serializedObject.Update();
                var newGuid = Guid.NewGuid();
                var bytes = newGuid.ToByteArray();
                a.intValue = BitConverter.ToInt32(bytes, 0);
                b.intValue = BitConverter.ToInt16(bytes, 4);
                c.intValue = BitConverter.ToInt16(bytes, 6);
                d.intValue = bytes[8];
                e.intValue = bytes[9];
                f.intValue = bytes[10];
                g.intValue = bytes[11];
                h.intValue = bytes[12];
                i.intValue = bytes[13];
                j.intValue = bytes[14];
                k.intValue = bytes[15];
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}