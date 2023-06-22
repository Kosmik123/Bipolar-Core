using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
#if UNITY_2021_1_OR_NEWER
    public class PointerMoveEventTrigger : PointerEventTrigger, IPointerMoveHandler
    {
        public void OnPointerMove(PointerEventData eventData)
        {
            Execute(eventData);
        }
    }
#endif
}
