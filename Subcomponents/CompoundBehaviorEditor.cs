using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace Bipolar.Subcomponents.Editor
{
    [CustomEditor(typeof(CompoundBehavior<>), editorForChildClasses: true)]
    public class CompoundBehaviorEditor : UnityEditor.Editor
    {
        private SerializedProperty componentsListProperty;
        private SubcomponentListEditor listEditor;

        private void OnEnable()
        {
            componentsListProperty = serializedObject.FindProperty("subcomponents");
            listEditor ??= new SubcomponentListEditor();
            listEditor.Init(componentsListProperty, serializedObject);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            bool changed = listEditor.Draw();
            if (changed) 
                serializedObject.ApplyModifiedProperties();
        }

        private void OnDisable()
        {
        }
    }


    //[CustomPropertyDrawer(typeof(ISubcomponent))]
    public class SubcomponentPropertyDrawer : PropertyDrawer
    {

    }

    public class SubcomponentListEditor
    {
        private SerializedProperty listProperty;
        private SerializedObject owner;

        public void Init(SerializedProperty listProperty, SerializedObject owner)
        {
            this.listProperty = listProperty;
            this.owner = owner;
        }

        public bool Draw()
        {
            bool changed = false;
            GUILayout.Space(10);
            int count = listProperty.arraySize;
            if (GUILayout.Button($"Add Component ({count})", EditorStyles.miniButton))
            {
                changed = true;
            }

            return changed;
        }
    }
}
