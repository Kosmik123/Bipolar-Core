using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public class DeselectEventTrigger : EventTrigger, IDeselectHandler
    {
        public void OnDeselect(BaseEventData eventData)
        {
            Execute(eventData);
        }
    }
}
