using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
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

        [SerializeField]
        private Component component;

        [SerializeReference, HideInInspector]
        private EventData[] eventsData;

        [System.Serializable]
        public class EventData
        {
            public string eventName;

            [SerializeReference]
            public UnityEventBase UnityEvent;
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(ComponentEvents))]
        public class ComponentEventsEditor : Editor
        {
            private static readonly Dictionary<Type, Type> unityEventTypesByArgumentType = new Dictionary<Type, Type>
            {
                [typeof(int)] = typeof(IntEvent),
                [typeof(float)] = typeof(FloatEvent),
                [typeof(string)] = typeof(StringEvent),
            };

            private readonly Dictionary<string, string> alreadySerializedEventNamesAndTypes = new Dictionary<string, string>();
            private readonly Dictionary<string, EventData> newEventDataByName = new Dictionary<string, EventData>();

            public static Type GetUnityEventType(Type argumentType)
            {
                if (unityEventTypesByArgumentType.TryGetValue(argumentType, out var unityEventType))
                    return unityEventType;

                return typeof(UnityEvent);
            }

            public override void OnInspectorGUI()
            {
                bool somethingChanged = false;
                EditorGUI.BeginChangeCheck();
                base.OnInspectorGUI();
                somethingChanged |= EditorGUI.EndChangeCheck();

                var component = serializedObject.FindProperty(nameof(ComponentEvents.component))?.objectReferenceValue;
                if (component == null)
                    return;

                alreadySerializedEventNamesAndTypes.Clear();
                newEventDataByName.Clear();

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
                        eventsDataProperty.InsertArrayElementAtIndex(i);
                        var newProperty = eventsDataProperty.GetArrayElementAtIndex(i);
                        newProperty.managedReferenceValue = CreateUnityEventData(componentEvent, componentType);
                    }
                }
                eventsDataProperty.arraySize = events.Length;


                /*
                //for (int i = 0; i < eventsDataProperty.arraySize; i++)
                //{
                //    var eventDatumProperty = eventsDataProperty.GetArrayElementAtIndex(i);
                //    string eventName = eventDatumProperty?.FindPropertyRelative(nameof(EventData.eventName))?.stringValue;
                //    string eventType = eventDatumProperty?.FindPropertyRelative(nameof(EventData.UnityEvent))?.type;

                //    alreadySerializedEventNamesAndTypes.Add(eventName, eventType);
                //}

                //for (int i = 0; i < events.Length; i++)
                //{
                //    //var arrayElementProperty = eventsProperty.GetArrayElementAtIndex(i);
                //    //if (arrayElementProperty.managedReferenceFullTypename.Length > 0)
                //    //    continue;

                //    var componentEventInfo = events[i];
                //    string eventName = componentEventInfo.Name;




                //    if (alreadySerializedEventNamesAndTypes.ContainsKey(eventName) == false)
                //    {
                //        var eventData = CreateUnityEventData(componentEventInfo, componentType);
                //        newEventDataByName.Add(eventName, eventData);
                //    }
                //}

                //eventsDataProperty.arraySize = events.Length; // to na pewno
                //for (int i = 0; i < eventsDataProperty.arraySize; i++)
                //{
                //    var serializedEvent = eventsDataProperty.GetArrayElementAtIndex(i);
                //    string eventDataName = serializedEvent?.FindPropertyRelative("eventName")?.stringValue;
                //    if (Array.FindIndex(events, ev => ev.Name == eventDataName) < 0)
                //    {
                //        serializedEvent.managedReferenceValue = newEventDataByName[eventDataName];
                //    }
                //}


                //    for (int i = 0; i < unityEventDataList.Count; i++)
                //{
                //    var singleEventProperty = eventsProperty.GetArrayElementAtIndex(i);
                //    singleEventProperty.managedReferenceValue = unityEventDataList[i];
                //}
                */

                EditorGUILayout.Space();
                EditorGUI.BeginChangeCheck();
                for (int i = 0; i < eventsDataProperty.arraySize; i++)
                {
                    var eventProperty = eventsDataProperty.GetArrayElementAtIndex(i);
                    var label = new GUIContent(ObjectNames.NicifyVariableName(GetEventDataName(eventProperty)));
                    EditorGUILayout.PropertyField(eventProperty.FindPropertyRelative(nameof(EventData.UnityEvent)), label);
                }
                somethingChanged |= EditorGUI.EndChangeCheck();
                if (somethingChanged)
                    serializedObject.ApplyModifiedProperties();
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
                    UnityEvent = unityEventInstance,
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
