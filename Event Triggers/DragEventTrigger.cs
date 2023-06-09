using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public class DragEventTrigger : PointerEventTrigger, IDragHandler
    {
        public void OnDrag(PointerEventData eventData)
        {
            Execute(eventData);
        }
    }
}
