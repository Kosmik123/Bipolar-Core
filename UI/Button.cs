using UnityEngine.EventSystems;

namespace Bipolar.UI
{
    public class Button : UnityEngine.UI.Button
    {
        public event System.Action OnClicked;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            OnClicked?.Invoke();
        }
    }
}
