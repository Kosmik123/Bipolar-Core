using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[System.Serializable]
public class SerializedInterface<T> where T : class
{
    [SerializeField]
    private Object value;
    public T Value
    {
        get => value as T;
        set => this.value = value as Object;
    }

    private SerializedInterface(T obj)
    {
        Value = obj;
    }

    public static implicit operator T(SerializedInterface<T> serializedInterface) => serializedInterface.Value;
    public static explicit operator SerializedInterface<T>(T obj) => new SerializedInterface<T>(obj);
}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SerializedInterface<>))]
public class SerializedInterfacePropertyDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        return base.CreatePropertyGUI(property);



    }
}
#endif
