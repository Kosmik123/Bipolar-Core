using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public class EndDragEventTrigger : PointerEventTrigger, IEndDragHandler
    {
        public void OnEndDrag(PointerEventData eventData)
        {
            Execute(eventData);
        }
    }
}
