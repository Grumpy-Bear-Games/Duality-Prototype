using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DualityGame.UI
{
    public static class MenuManagement
    {
        private class MenuStackEntry
        {
            private readonly MenuBase menu;
            private Selectable _focused;

            public MenuStackEntry(MenuBase previousMenu) => menu = previousMenu;

            public void Hide()
            {
                _focused = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
                menu.gameObject.SetActive(false);
            }

            public void Restore()
            {
                menu.gameObject.SetActive(true);
                _focused.Select();
            }
        }

        private static readonly Stack<MenuStackEntry> _menuStack = new();
     
        public static void Show(MenuBase menu, Selectable firstFocused)
        {
            if (_menuStack.Count > 0) {
                _menuStack.Peek().Hide();
            }
            
            _menuStack.Push(new MenuStackEntry(menu));
            menu.gameObject.SetActive(true);
            firstFocused.Select();
        }

        public static void Back()
        {
            if (_menuStack.Count == 0) return;
            _menuStack.Pop().Hide();

            if (_menuStack.Count == 0) return;
            var next = _menuStack.Peek();
            next.Restore();
        }
    }
}
