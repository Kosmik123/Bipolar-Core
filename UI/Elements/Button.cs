using UnityEngine;
using UnityEngine.EventSystems;

namespace Bipolar.UI
{
    public interface ISelectable
    {
        bool IsSelected { get; }
        bool IsHighlighted { get; }
        bool IsPressed { get; }
    }

    public interface IButton : ISelectable
    {
        event System.Action OnClicked;
    }

    public class Button : UnityEngine.UI.Button, IButton
    {
        public bool IsSelected => currentSelectionState == SelectionState.Selected;
        bool ISelectable.IsHighlighted => IsHighlighted();
        bool ISelectable.IsPressed => IsPressed();

        public event System.Action OnClicked;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            OnClicked?.Invoke();
        }
    }

}
