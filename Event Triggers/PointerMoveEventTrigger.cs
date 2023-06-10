using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public class PointerMoveEventTrigger : PointerEventTrigger, IPointerMoveHandler
    {
        public void OnPointerMove(PointerEventData eventData)
        {
            Execute(eventData);
        }
    }
}
