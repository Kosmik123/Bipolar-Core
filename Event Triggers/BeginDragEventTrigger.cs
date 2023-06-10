using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public class BeginDragEventTrigger : PointerEventTrigger, IBeginDragHandler
    {
        public void OnBeginDrag(PointerEventData eventData)
        {
            Execute(eventData);
        }
    }
}
