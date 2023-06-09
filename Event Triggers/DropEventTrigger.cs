using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public class DropEventTrigger : PointerEventTrigger, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            Execute(eventData);
        }
    }
}
