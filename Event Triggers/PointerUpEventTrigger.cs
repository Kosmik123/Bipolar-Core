using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public class PointerUpEventTrigger : PointerEventTrigger, IPointerUpHandler
    {
        public void OnPointerUp(PointerEventData eventData)
        {
            Execute(eventData);
        }
    }
}
