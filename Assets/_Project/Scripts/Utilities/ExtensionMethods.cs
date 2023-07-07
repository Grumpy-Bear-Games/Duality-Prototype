using UnityEngine.UIElements;

namespace DualityGame.Utilities
{
    public static class ExtensionMethods
    {
        public static void PreventLoosingFocus(this VisualElement value)
        {
            value.RegisterCallback<PointerDownEvent>(evt =>
            {
                evt.StopImmediatePropagation();
                evt.PreventDefault();
            });
        }
    }
}
