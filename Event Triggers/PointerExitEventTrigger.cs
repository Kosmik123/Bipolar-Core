using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public class PointerExitEventTrigger : PointerEventTrigger, IPointerExitHandler
    {
        public void OnPointerExit(PointerEventData eventData)
        {
            Execute(eventData);
        }
    }
}
