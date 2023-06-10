using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public class PointerDownEventTrigger : PointerEventTrigger, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            Execute(eventData);
        }
    }
}
