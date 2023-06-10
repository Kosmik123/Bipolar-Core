using UnityEngine.EventSystems;

namespace Bipolar.EventTriggers
{
    public class InitializePotentialDragEventTrigger : PointerEventTrigger, IInitializePotentialDragHandler
    {
        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            Execute(eventData);
        }
    }
}
