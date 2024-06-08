using UnityEngine;

namespace Bipolar
{
    [SelectionBase]
    public class SelectionBase : MonoBehaviour
    { }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(SelectionBase))]
    public class SelectionBaseEditor : UnityEditor.Editor 
    {
        public override void OnInspectorGUI()
        { }
    }
#endif
}
