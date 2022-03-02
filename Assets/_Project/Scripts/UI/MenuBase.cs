using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DualityGame.UI
{
    public abstract class MenuBase : MonoBehaviour, ICancelHandler
    {
        private class PreviousState
        {
            public readonly MenuBase Menu;
            public readonly Selectable Focused;

            public PreviousState(MenuBase previousMenu, Selectable previousFocused)
            {
                Menu = previousMenu;
                Focused = previousFocused;
            }
        }

        private static readonly Stack<PreviousState> _previousStates = new();

        [SerializeField] private Selectable _firstSelectable;
       
        [UsedImplicitly]
        public void Show(MenuBase menu)
        {
            var previousFocus = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
            var p = new PreviousState(this, previousFocus);
            _previousStates.Push(p);
            gameObject.SetActive(false);
            menu.gameObject.SetActive(true);
            menu._firstSelectable.Select();
        }

        [UsedImplicitly]
        public void Back()
        {
            if (_previousStates.Count < 1) return;
            var p = _previousStates.Pop();
            gameObject.SetActive(false);
            p.Menu.gameObject.SetActive(true);
            p.Focused.Select();
        }

        void ICancelHandler.OnCancel(BaseEventData eventData) => Back();
    }
}
