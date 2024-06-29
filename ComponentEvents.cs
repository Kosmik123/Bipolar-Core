using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using Bipolar.UI;
using System.Linq.Expressions;
using System.Linq;
using System.Diagnostics.Contracts;




#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Bipolar
{
    public sealed class ComponentEvents : MonoBehaviour
    {
        public class IntEvent : UnityEvent<int> { }
        public class StringEvent : UnityEvent<string> { }
        public class FloatEvent : UnityEvent<float> { }

        private static readonly Dictionary<Type, Type> unityEventTypesByArgumentType = new Dictionary<Type, Type>
        {
            //[typeof(int)] = typeof(IntEvent),
            //[typeof(float)] = typeof(FloatEvent),
            //[typeof(string)] = typeof(StringEvent),
        };

        public static Type GetUnityEventType(Type argumentType)
        {
            if (unityEventTypesByArgumentType.TryGetValue(argumentType, out var unityEventType))
                return unityEventType;

            return typeof(UnityEvent);
        }

        [SerializeField]
        private Component component;

        [SerializeReference, HideInInspector]
        private EventData[] eventsData;

        [System.Serializable]
        public class EventData
        {
            #region serialized
            public string eventName;
            [SerializeReference]
            public UnityEventBase unityEvent;
            #endregion
            #region runtime
            public EventInfo EventInfo { get; set; }
            public Delegate InvokeDelegate { get; set; }
            #endregion
        }

        private void Awake()
        {
            if (component == null)
                return;

            var componentType = component.GetType();
            var events = componentType.GetEvents();
            int count = Mathf.Min(events.Length, eventsData.Length);
            var invokeUnityEventInfo = typeof(UnityEvent).GetMethod(nameof(UnityEvent.Invoke));

            for (int i = 0; i < count; i++)
            {
                var eventInfo = events[i];
                var unityEventData = eventsData[i];
                unityEventData.EventInfo = eventInfo;

                Type eventHandlerType = eventInfo.EventHandlerType;
                MethodInfo actionInvoke = eventHandlerType.GetMethod(nameof(Action.Invoke));
                ParameterInfo[] parameters = actionInvoke.GetParameters();
                ParameterExpression[] parameterExpressions = parameters.Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToArray();

                Expression instanceExpression = Expression.Constant(unityEventData.unityEvent);
                Expression body = Expression.Call(instanceExpression, invokeUnityEventInfo);

                LambdaExpression lambda = Expression.Lambda(eventHandlerType, body, parameterExpressions);
                Delegate handlerDelegate = lambda.Compile();

                unityEventData.InvokeDelegate = handlerDelegate;

                //eventInfo.AddEventHandler(component, handlerDelegate);
                //var del = Delegate.CreateDelegate(typeof(Action), unityEventData.UnityEvent, invokeUnityEventInfo);
            }
        }

        private void OnEnable()
        {
            
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(ComponentEvents))]
        public class ComponentEventsEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                bool somethingChanged = false;
                EditorGUI.BeginChangeCheck();
                base.OnInspectorGUI();
                somethingChanged |= EditorGUI.EndChangeCheck();

                var component = serializedObject.FindProperty(nameof(ComponentEvents.component))?.objectReferenceValue;
                if (component == null)
                    return;

                var componentType = component.GetType();
                var events = componentType.GetEvents();
                var eventsToSerialize = new List<EventInfo>(events);

                var eventsDataProperty = serializedObject.FindProperty(nameof(ComponentEvents.eventsData));
                eventsDataProperty.arraySize = Mathf.Max(eventsDataProperty.arraySize, events.Length);
                for (int i = 0; i < events.Length; i++)
                {
                    var componentEvent = events[i];
                    int serializedEventIndex = FindIndex(eventsDataProperty, CompareNames);
                    bool CompareNames(SerializedProperty property) =>
                        GetEventDataName(property) == componentEvent.Name;

                    if (serializedEventIndex >= 0)
                    {
                        if (serializedEventIndex != i)
                            eventsDataProperty.MoveArrayElement(serializedEventIndex, i);
                    }
                    else
                    {
                        var newProperty = InsertArrayElementAtIndex(eventsDataProperty, i);
                        newProperty.managedReferenceValue = CreateUnityEventData(componentEvent, componentType);
                    }
                }
                eventsDataProperty.arraySize = events.Length;

                EditorGUILayout.Space();
                EditorGUI.BeginChangeCheck();
                for (int i = 0; i < eventsDataProperty.arraySize; i++)
                {
                    var eventProperty = eventsDataProperty.GetArrayElementAtIndex(i);
                    var label = new GUIContent(ObjectNames.NicifyVariableName(GetEventDataName(eventProperty)));
                    EditorGUILayout.PropertyField(eventProperty.FindPropertyRelative(nameof(EventData.unityEvent)), label);
                }
                somethingChanged |= EditorGUI.EndChangeCheck();
                if (somethingChanged)
                    serializedObject.ApplyModifiedProperties();
            }

            private static SerializedProperty InsertArrayElementAtIndex(SerializedProperty arrayProperty, int i)
            {
                arrayProperty.InsertArrayElementAtIndex(i);
                var addedElement = arrayProperty.GetArrayElementAtIndex(i);
                return addedElement;
            }

            private static string GetEventDataName(SerializedProperty property)
            {
                return property?.FindPropertyRelative(nameof(EventData.eventName))?.stringValue;
            }

            private static EventData CreateUnityEventData(EventInfo componentEvent, Type componentType)
            {
                var eventHandlerType = componentEvent.EventHandlerType;
                var genericTypeArguments = eventHandlerType.GenericTypeArguments;

                Type eventDataType = typeof(UnityEvent);
                if (genericTypeArguments != null && genericTypeArguments.Length != 0)
                {
                    int argumentIndex = genericTypeArguments[0] == componentType ? 1 : 0;
                    if (argumentIndex > 0 && genericTypeArguments.Length > 1)
                    {
                        var argumentType = genericTypeArguments[argumentIndex];
                        eventDataType = GetUnityEventType(argumentType);
                    }
                }

                var unityEventInstance = (UnityEventBase)Activator.CreateInstance(eventDataType);
                return new EventData()
                {
                    unityEvent = unityEventInstance,
                    eventName = componentEvent.Name,
                };
            }

            public static int FindIndex(SerializedProperty arrayProperty, Predicate<SerializedProperty> predicate)
            {
                if (arrayProperty == null)
                    return -1;

                for (int i = 0; i < arrayProperty.arraySize; i++)
                {
                    var element = arrayProperty.GetArrayElementAtIndex(i);
                    if (predicate(element))
                        return i;
                }

                return -1;
            }
        }

#endif
    }
}
