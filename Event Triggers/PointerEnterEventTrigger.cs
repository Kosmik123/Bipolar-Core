using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public class PointerEnterEventTrigger : PointerEventTrigger, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            Execute(eventData);
        }
    }
}
