using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public class SubmitEventTrigger : EventTrigger, ISubmitHandler
    {
        public void OnSubmit(BaseEventData eventData)
        {
            Execute(eventData);
        }
    }
}
