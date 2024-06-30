using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using System.Linq.Expressions;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Bipolar
{
    public sealed class ComponentEvents : MonoBehaviour
    {
        private static readonly Dictionary<Type, Type> eventDataTypesByArgumentType = new Dictionary<Type, Type>
        {
            [typeof(int)] = typeof(EventDataInt),
            [typeof(bool)] = typeof(EventDataBool),
            [typeof(float)] = typeof(EventDataFloat),
            [typeof(string)] = typeof(EventDataString),
        };

        public static Type GetEventDataType(Type argumentType)
        {
            if (eventDataTypesByArgumentType.TryGetValue(argumentType, out var unityEventType))
                return unityEventType;

            return null;
        }

        [SerializeField]
        private Component component;

        [SerializeReference]
        private BaseEventData[] eventsData;

        [System.Serializable]
        private abstract class BaseEventData
        {
            public string eventName;
            public abstract UnityEventBase UnityEvent { get; }
            public EventInfo EventInfo { get; set; }
            public Delegate InvokeDelegate { get; set; }

            internal void Clear()
            {
                eventName = null;
                InvokeDelegate = null;
                EventInfo = null;
                UnityEvent.RemoveAllListeners();
            }
        }

        [System.Serializable]
        private class EventData : BaseEventData
        {
            [SerializeField]
            internal UnityEvent unityEvent;
            public override UnityEventBase UnityEvent => unityEvent;
        }

        [System.Serializable]
        private class EventDataInt : BaseEventData
        {
            [SerializeField]
            internal UnityEvent<int> unityEvent;
            public override UnityEventBase UnityEvent => unityEvent;
        }

        [System.Serializable]
        private class EventDataFloat : BaseEventData
        {
            [SerializeField]
            internal UnityEvent<float> unityEvent;
            public override UnityEventBase UnityEvent => unityEvent;
        }

        [System.Serializable]
        private class EventDataString : BaseEventData
        {
            [SerializeField]
            internal UnityEvent<string> unityEvent;
            public override UnityEventBase UnityEvent => unityEvent;
        }

        [System.Serializable]
        private class EventDataBool : BaseEventData
        {
            [SerializeField]
            internal UnityEvent<bool> unityEvent;
            public override UnityEventBase UnityEvent => unityEvent;
        }

        private void Awake()
        {
            if (component == null)
            {
                Destroy(this);
                return;
            }

            var componentType = component.GetType();
            var events = componentType.GetEvents();
            int count = Mathf.Min(events.Length, eventsData.Length);

            for (int i = 0; i < count; i++)
            {
                var eventInfo = events[i];
                var unityEventData = eventsData[i];
                unityEventData.EventInfo = eventInfo;

                ParameterExpression[] eventParameters = GetEventParameterExpressions(eventInfo);

                Expression instanceExpression = Expression.Constant(unityEventData.UnityEvent);
                var invokeUnityEventInfo = unityEventData.UnityEvent.GetType().GetMethod(nameof(UnityEvent.Invoke));
                var unityEventArguments = invokeUnityEventInfo.GetParameters();

                Expression body = null;
                int possibleParametersCount = Mathf.Min(2, eventParameters.Length);
                if (unityEventArguments.Length > 0)
                {
                    for (int a = 0; a < possibleParametersCount; a++)
                    {
                        var argumentType = eventParameters[a].Type;
                        if (eventDataTypesByArgumentType.ContainsKey(argumentType))
                        {
                            Expression passedParameter = eventParameters[a];
                            body = Expression.Call(instanceExpression, invokeUnityEventInfo, passedParameter);
                            break;
                        }
                    }
                }

                if (body == null)
                {
                    body = Expression.Call(instanceExpression, invokeUnityEventInfo);
                }

                LambdaExpression lambda = Expression.Lambda(eventInfo.EventHandlerType, body, eventParameters);
                unityEventData.InvokeDelegate = lambda.Compile();
            }
            Debug.Log(duration);
        }

        private static ParameterExpression[] GetEventParameterExpressions(EventInfo eventInfo)
        {
            Type eventHandlerType = eventInfo.EventHandlerType;
            MethodInfo actionInvoke = eventHandlerType.GetMethod(nameof(Action.Invoke));
            ParameterExpression[] eventParameters = actionInvoke.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToArray();
            return eventParameters;
        }

        private void OnEnable()
        {
            foreach (var eventDatum in eventsData)
                eventDatum?.EventInfo?.AddEventHandler(component, eventDatum.InvokeDelegate);
        }

        private void OnDisable()
        {
            foreach (var eventDatum in eventsData)
                eventDatum?.EventInfo?.AddEventHandler(component, eventDatum.InvokeDelegate);
        }

        private void OnDestroy()
        {
            for (int i = 0; i < eventsData.Length; i++)
            {
                eventsData[i].Clear();
                eventsData[i] = null;
            }
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(ComponentEvents))]
        public class ComponentEventsEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                //base.OnInspectorGUI();

                using (new EditorGUI.DisabledScope(true))
                    EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);

                var componentProperty = serializedObject.FindProperty(nameof(ComponentEvents.component));
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(componentProperty);
                if (EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();

                var component = componentProperty?.objectReferenceValue;
                if (component == null)
                    return;

                var componentType = component.GetType();
                var events = componentType.GetEvents();
                var eventsToSerialize = new List<EventInfo>(events);

                bool somethingChanged = false;
                var eventsDataProperty = serializedObject.FindProperty(nameof(ComponentEvents.eventsData));
                eventsDataProperty.arraySize = Mathf.Max(eventsDataProperty.arraySize, events.Length);
                for (int i = 0; i < events.Length; i++)
                {
                    var componentEvent = events[i];
                    int serializedEventIndex = FindIndex(eventsDataProperty, CompareNames);
                    bool CompareNames(SerializedProperty property) =>
                        GetEventDataName(property) == componentEvent.Name;

                    if (serializedEventIndex < 0)
                    {
                        var newProperty = InsertArrayElementAtIndex(eventsDataProperty, i);
                        CreateNewEventDataInProperty(newProperty, componentType, componentEvent);

                        //var unityEventProperty = newProperty.FindPropertyRelative("unityEvent");
                        //unityEventProperty.managedReferenceValue = CreateUnityEvent(componentEvent, componentType);

                        somethingChanged = true;
                    }
                    else
                    {
                        var singleEventProperty = eventsDataProperty.GetArrayElementAtIndex(i);
                        var correctEventType = GetEventDataType(componentEvent.EventHandlerType, componentType);
                        if (CheckType(singleEventProperty, correctEventType) == false)
                        {
                            CreateNewEventDataInProperty(singleEventProperty, componentType, componentEvent);
                            somethingChanged = true;
                        }

                        if (serializedEventIndex != i)
                        {
                            eventsDataProperty.MoveArrayElement(serializedEventIndex, i);
                            somethingChanged = true;
                        }
                    }
                }
                eventsDataProperty.arraySize = events.Length;

                EditorGUILayout.Space();
                EditorGUI.BeginChangeCheck();
                for (int i = 0; i < eventsDataProperty.arraySize; i++)
                {
                    var eventProperty = eventsDataProperty.GetArrayElementAtIndex(i);
                    var unityEventProperty = eventProperty?.FindPropertyRelative(nameof(EventData.unityEvent));
                    if (unityEventProperty != null)
                    {
                        var label = new GUIContent(ObjectNames.NicifyVariableName(GetEventDataName(eventProperty)));
                        EditorGUILayout.PropertyField(unityEventProperty, label);
                    }
                }
                somethingChanged |= EditorGUI.EndChangeCheck();
                if (somethingChanged)
                    serializedObject.ApplyModifiedProperties();
            }

            private static void CreateNewEventDataInProperty(SerializedProperty property, Type componentType, EventInfo componentEvent)
            {
                property.managedReferenceValue = CreateEventData(componentEvent, componentType);
                property.FindPropertyRelative(nameof(EventData.eventName)).stringValue = componentEvent.Name;
            }

            private static bool CheckType(SerializedProperty eventDataProperty, Type correctEventType)
            {
                string correctEventTypeName = correctEventType.Name;
                string eventTypeName = eventDataProperty.type;
                int realTypeNameStart = eventTypeName.IndexOf('<') + 1;
                int realTypeNameLength = eventTypeName.IndexOf('>') - realTypeNameStart;
                eventTypeName = eventTypeName.Substring(realTypeNameStart, realTypeNameLength);
                bool isCorrect = eventTypeName == correctEventTypeName;
                return isCorrect;
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

            private static BaseEventData CreateEventData(EventInfo componentEvent, Type componentType)
            {
                var eventHandlerType = componentEvent.EventHandlerType;
                Type eventDataType = GetEventDataType(eventHandlerType, componentType);

                var unityEventInstance = (BaseEventData)Activator.CreateInstance(eventDataType);
                return unityEventInstance;
            }

            private static Type GetEventDataType(Type eventHandlerType, Type componentType)
            {
                var methodInfo = eventHandlerType.GetMethod(nameof(Action.Invoke));
                var eventParameters = methodInfo.GetParameters();

                int possibleParametersCount = Mathf.Min(2, eventParameters.Length);
                for (int i = 0; i < possibleParametersCount; i++)
                {
                    var argumentType = eventParameters[i].ParameterType;
                    var eventDataType = ComponentEvents.GetEventDataType(argumentType);
                    if (eventDataType != null)
                        return eventDataType;
                }

                return typeof(EventData);
            }

            public static int FindIndex(SerializedProperty arrayProperty, Predicate<SerializedProperty> predicate)
            {
                if (arrayProperty != null)
                {
                    for (int i = 0; i < arrayProperty.arraySize; i++)
                    {
                        var element = arrayProperty.GetArrayElementAtIndex(i);
                        if (predicate(element))
                            return i;
                    }
                }

                return -1;
            }
        }
#endif
    }
}
