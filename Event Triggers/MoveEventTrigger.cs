using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public class MoveEventTrigger : AxisEventTrigger, IMoveHandler
    {
        public void OnMove(AxisEventData eventData)
        {
            Execute(eventData);
        }
    }
}
