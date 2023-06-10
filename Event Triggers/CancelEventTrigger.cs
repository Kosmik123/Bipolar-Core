using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public class CancelEventTrigger : EventTrigger, ICancelHandler
    {
        public void OnCancel(BaseEventData eventData)
        {
            Execute(eventData);
        }
    }

}
