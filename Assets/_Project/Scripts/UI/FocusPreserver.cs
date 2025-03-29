using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class FocusPreserver : MonoBehaviour
    {
        private Focusable _lastFocusable;

        private void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            Application.focusChanged += hasFocus =>
            {
                if (hasFocus) return;
                _lastFocusable = root.focusController?.focusedElement;
            };

            root.RegisterCallback<BlurEvent>(evt =>
            {
                if (_lastFocusable == null) return;
                if (evt.relatedTarget != null) return;
                root.schedule.Execute(() =>
                {
                    if (_lastFocusable == null) return;
                    _lastFocusable.Focus();
                    _lastFocusable = null;
                });
            }, TrickleDown.TrickleDown);
        }
    }
}
