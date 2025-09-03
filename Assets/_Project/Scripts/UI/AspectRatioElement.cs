using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    [UxmlElement]
    public partial class AspectRatioElement : Image
    {
        #region Attributes

        [UxmlAttribute]
        [CreateProperty]
        public Vector2 Ratio
        {
            get => _ratio;
            set
            {
                if (value == _ratio) return;
                _ratio = value;
                ApplyAspect();
            }
        }
        #endregion

        #region Private members
        private Vector2 _ratio = new(100, 100);
        private IVisualElementScheduledItem _scheduled;
        #endregion

        public AspectRatioElement()
        {
            RegisterCallback<AttachToPanelEvent>(_ => ApplyAspect());
            RegisterCallback<GeometryChangedEvent>(_ => ApplyAspect());
        }

        private void ApplyAspect()
        {
            if (panel == null) return;

            if (_ratio.x <= 0.0f || _ratio.y <= 0.0f)
            {
                style.height = new StyleLength(StyleKeyword.Initial);
                return;
            }

            // Width comes from style/flex; we compute height from it
            var currentWidth = resolvedStyle.width;
            var currentHeight = resolvedStyle.height;

            if (float.IsNaN(currentWidth) || currentWidth <= 0) return;

            var calculatedHeight = (_ratio.y / _ratio.x) * currentWidth;

            if (Mathf.Approximately(calculatedHeight, currentHeight)) return;

            //Debug.Log($"[{name}] Applying style.height = {calculatedHeight}");
            style.height = calculatedHeight;
        }
    }
}
