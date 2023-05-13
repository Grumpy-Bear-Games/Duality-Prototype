using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    public static class FocusHelper
    {
        private static readonly Stack<Focusable> _focusStack = new();
        
        public static Focusable GetFocusedElement()
        {
            var eventSystem = EventSystem.current;
            if (eventSystem == null)
            {
                return null;
            }
 
            var selectedGameObject = eventSystem.currentSelectedGameObject;
            if (selectedGameObject == null)
            {
                return null;
            }
   
            var panelEventHandler = selectedGameObject.GetComponent<PanelEventHandler>();
            return panelEventHandler != null ? panelEventHandler.panel.focusController.focusedElement : null;
        }

        public static void Push()
        {
            var focused = GetFocusedElement();
            if (focused == null) throw new NullReferenceException();
            if (_focusStack.TryPeek(out var previousFocused) && previousFocused == focused) return;
            _focusStack.Push(focused);
        }

        public static void Pop()
        {
            if (!_focusStack.TryPop(out var previousFocused)) throw new IndexOutOfRangeException();
            previousFocused.Focus();
        }
    }
}
