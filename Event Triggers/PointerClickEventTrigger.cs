using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public class PointerClickEventTrigger : PointerEventTrigger, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Execute(eventData);
        }
    }
}
