using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Bipolar
{
	public class ComponentEvents : MonoBehaviour
    {
        [SerializeField]
        private Component component;

        [SerializeReference]
        private UnityEventBase[] events;
    }
#if UNITY_EDITOR
	[CustomEditor(typeof(ComponentEvents))]
    public class ComponentEventsEditor : Editor
    {
        private static readonly Type[] unityEventTypesByGenericParametersCount =
        {
            typeof(UnityEvent),
            //typeof(UnityEvent<>),
            //typeof(UnityEvent<,>),
            //typeof(UnityEvent<,,>),
            //typeof(UnityEvent<,,,>),
        };

		public override void OnInspectorGUI()
		{
			var eventsProperty = serializedObject.FindProperty("events");
			var componentProperty = serializedObject.FindProperty("component");
			var component = componentProperty.objectReferenceValue;
			if (component == null)
				return;

			var componentType = component.GetType();
			var events = componentType.GetEvents();
			bool somethingChanged = false;

			var eventPropertiesList = new List<SerializedProperty>();
			
			for (int i = 0; i < events.Length; i++)
			{
				var arrayElementProperty = eventsProperty.GetArrayElementAtIndex(i);
				if (arrayElementProperty.managedReferenceFullTypename.Length > 0)
					continue;

				var componentEvent = events[i];
				var eventHandlerType = componentEvent.EventHandlerType;

				var genericTypeArguments = new List<Type>(eventHandlerType.GenericTypeArguments);
				if (genericTypeArguments.Count > 1 && genericTypeArguments[0] == componentType)
					genericTypeArguments.RemoveAt(0);

				int genericTypesCount = Mathf.Min(genericTypeArguments.Count, unityEventTypesByGenericParametersCount.Length - 1);
				var unityEventType = unityEventTypesByGenericParametersCount[0];
				if (genericTypesCount > 0)
					;// unityEventType = unityEventType.MakeGenericType(genericTypeArguments.ToArray());

				var unityEventInstance = Activator.CreateInstance(unityEventType);
				arrayElementProperty.managedReferenceValue = unityEventInstance;
				somethingChanged = true;
			}

			eventsProperty.arraySize = eventPropertiesList.Count;
			for (int i = 0; i < eventPropertiesList.Count; i++)
			{

			}

			if (somethingChanged)
				serializedObject.ApplyModifiedProperties();

			EditorGUI.BeginChangeCheck();
			for (int i = 0; i < eventsProperty.arraySize; i++)
			{
				var eventProperty = eventsProperty.GetArrayElementAtIndex(i);
				var label = new GUIContent(ObjectNames.NicifyVariableName(events[i].Name));
				EditorGUILayout.PropertyField(eventProperty, label);
			}
			if (EditorGUI.EndChangeCheck())
				serializedObject.ApplyModifiedProperties();
		}
	}
#endif
}
