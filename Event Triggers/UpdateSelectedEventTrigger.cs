using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public class UpdateSelectedEventTrigger : EventTrigger, IUpdateSelectedHandler
    {
        public void OnUpdateSelected(BaseEventData eventData)
        {
            Execute(eventData);
        }
    }
}
