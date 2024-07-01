using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq.Expressions;
using System.Linq;

namespace Bipolar.Prototyping.ComponentEvents
{
    public sealed partial class ComponentEvents : MonoBehaviour
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
        internal Component component;

        [SerializeReference]
        internal EventDataBase[] eventsData;

        private void Awake()
        {
            Debug.Log("Let's check it this works");
            if (component == null)
            {
                enabled = false;
                Destroy(this);
                return;
            }

            var componentType = component.GetType();
            var events = componentType.GetEvents();
            Debug.Log("We got type and events");
            int count = Mathf.Min(events.Length, eventsData.Length);

            Debug.Log("Start iterating events");
            for (int i = 0; i < count; i++)
            {
                Debug.Log($"Initialize event: {i}");
                var unityEventData = eventsData[i];
                var eventInfo = componentType.GetEvent(unityEventData.eventName);
                unityEventData.EventInfo = eventInfo;

                Debug.Log($"Event is: {eventInfo}");
                ParameterExpression[] eventParameters = GetEventParameterExpressions(eventInfo);

                Expression instanceExpression = Expression.Constant(unityEventData.UnityEvent);
                var invokeUnityEventInfo = unityEventData.UnityEvent.GetType().GetMethod(nameof(UnityEvent.Invoke));
                var unityEventParameters = invokeUnityEventInfo.GetParameters();

                Debug.Log("We got parameters and events");
                Expression body = null;
                Debug.Log($"The event has {eventParameters.Length} parameters");

                int possibleParametersCount = Mathf.Min(2, eventParameters.Length);
                Debug.Log($"UnityEvent takes {unityEventParameters.Length} parameters");

                if (unityEventParameters.Length > 0)
                {
                    for (int a = 0; a < possibleParametersCount; a++)
                    {
                        Debug.Log($"Lets check {a} parameter");
                        var argumentType = eventParameters[a].Type;
                        Debug.Log($"Its type is {argumentType}");
                        if (eventDataTypesByArgumentType.ContainsKey(argumentType))
                        {
                            Debug.Log($"UnityEvent found! We can create a body that passes a paremeter");
                            Expression passedParameter = eventParameters[a];
                            body = Expression.Call(instanceExpression, invokeUnityEventInfo, passedParameter);
                            break;
                        }
                        Debug.Log($"But none UnityEvent takes {argumentType}");
                    }
                }

                Debug.Log("Lets check if body is null");
                if (body == null)
                {
                    Debug.Log("It is!");
                    body = Expression.Call(instanceExpression, invokeUnityEventInfo);
                }
                Debug.Log($"Function body is {body}");
                Debug.Log($"Event handler type is {eventInfo.EventHandlerType}");
                Debug.Log($"Event type is {eventInfo.EventHandlerType}");

                LambdaExpression lambda = Expression.Lambda(eventInfo.EventHandlerType, body, eventParameters);

                Debug.Log($"We created lambda: {lambda}");
                Delegate compiledDelegate = lambda.Compile();

                Debug.Log($"Compiled Delegate is: {compiledDelegate}! Hooray!");
                unityEventData.InvokeDelegate = compiledDelegate;
            }
        }

        private static ParameterExpression[] GetEventParameterExpressions(EventInfo eventInfo)
        {
            Type eventHandlerType = eventInfo.EventHandlerType;
            Debug.Log($"Type of Event Handler is {eventHandlerType}");
            MethodInfo invokeMethodInfo = eventHandlerType.GetMethod(nameof(Action.Invoke));
            Debug.Log($"Method that is invoked is {invokeMethodInfo}");
            ParameterInfo[] parameterInfos = invokeMethodInfo.GetParameters();
            Debug.Log($"Method linked to this event should have {parameterInfos.Length} parameters {parameterInfos}");
            ParameterExpression[] eventParameters = parameterInfos.Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToArray();
            Debug.Log($"Which gives us {eventParameters.Length} parameterExpressions: {eventParameters}");

            return eventParameters;
        }

        private void OnEnable()
        {
            Debug.Log($"SUBSCRIBE");
            foreach (var eventDatum in eventsData)
            {
                Debug.Log($"Iterate {eventDatum}");
                EventInfo eventInfo = eventDatum.EventInfo;
                Debug.Log($"It's event info is {eventInfo}");
                Debug.Log($"We can add event handler to it");
                eventInfo.AddEventHandler(component, eventDatum.InvokeDelegate);
                Debug.Log($"Added");
            }
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
    }
}
