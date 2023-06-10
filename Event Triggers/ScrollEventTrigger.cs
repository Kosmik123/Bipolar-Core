using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public class ScrollEventTrigger : PointerEventTrigger, IScrollHandler
    {
        public void OnScroll(PointerEventData eventData)
        {
            Execute(eventData);
        }
    }
}
