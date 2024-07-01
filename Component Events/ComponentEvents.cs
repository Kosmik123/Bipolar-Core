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
            if (component == null)
            {
                enabled = false;
                Destroy(this);
                return;
            }

            var componentType = component.GetType();
            var events = componentType.GetEvents();
            int count = Mathf.Min(events.Length, eventsData.Length);
            for (int i = 0; i < count; i++)
            {
                var unityEventData = eventsData[i];
                var eventInfo = componentType.GetEvent(unityEventData.eventName);
                unityEventData.EventInfo = eventInfo;

                ParameterExpression[] eventParameters = GetEventParameterExpressions(eventInfo);

                Expression instanceExpression = Expression.Constant(unityEventData.UnityEvent);
                var invokeUnityEventInfo = unityEventData.UnityEvent.GetType().GetMethod(nameof(UnityEvent.Invoke));
                var unityEventParameters = invokeUnityEventInfo.GetParameters();

                Expression body = null;
                int possibleParametersCount = Mathf.Min(2, eventParameters.Length);
                if (unityEventParameters.Length > 0)
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

                Delegate compiledDelegate = lambda.Compile();
                unityEventData.InvokeDelegate = compiledDelegate;
            }
        }

        private static ParameterExpression[] GetEventParameterExpressions(EventInfo eventInfo)
        {
            Type eventHandlerType = eventInfo.EventHandlerType;
            MethodInfo invokeMethodInfo = eventHandlerType.GetMethod(nameof(Action.Invoke));
            ParameterInfo[] parameterInfos = invokeMethodInfo.GetParameters();
            ParameterExpression[] eventParameters = parameterInfos.Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToArray();
            return eventParameters;
        }

        private void OnEnable()
        {
            foreach (var eventDatum in eventsData)
            {
                EventInfo eventInfo = eventDatum.EventInfo;
                eventInfo.AddEventHandler(component, eventDatum.InvokeDelegate);
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
