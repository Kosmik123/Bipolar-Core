#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEditor;
using UnityEngine;

namespace Bipolar.Prototyping.ComponentEvents
{
    [CustomEditor(typeof(ComponentEvents))]
    public class ComponentEventsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            using (new EditorGUI.DisabledScope(true))
                EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);

            if (PlayerSettings.GetScriptingBackend(EditorUserBuildSettings.selectedBuildTargetGroup) == ScriptingImplementation.IL2CPP)
            {
                EditorGUILayout.HelpBox("Component Events is not supported in IL2CPP scripting backend", MessageType.Error);
                //return;
            }

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

        private static EventDataBase CreateEventData(EventInfo componentEvent, Type componentType)
        {
            var eventHandlerType = componentEvent.EventHandlerType;
            Type eventDataType = GetEventDataType(eventHandlerType, componentType);

            var unityEventInstance = (EventDataBase)Activator.CreateInstance(eventDataType);
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

}
#endif
