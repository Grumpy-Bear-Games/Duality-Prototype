using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.VFX
{
    //[RequireComponent(typeof(UIDocument))]
    public class UIToolkitScreenFader : UIToolkitScreenFaderBase
    {
        [SerializeField] private StyleSheet _styleSheet;
        [SerializeField] private ScreenFader.Direction _initialState;

        protected override void Awake()
        {
            base.Awake();
            if (_styleSheet) _root.styleSheets.Add(_styleSheet);
            
            if (_initialState == ScreenFader.Direction.FadeIn) FadeIn();
        }
    }
}
