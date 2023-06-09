using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public class SelectEventTrigger : EventTrigger, ISelectHandler
    {
        public void OnSelect(BaseEventData eventData)
        {
            Execute(eventData);
        }
    }
}
