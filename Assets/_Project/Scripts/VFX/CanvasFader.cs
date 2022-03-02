using Games.GrumpyBear.Core;
using UnityEngine;

namespace DualityGame.VFX
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasFader : ScreenFaderProviderBase
    {
        [SerializeField] private bool _controlInteractable = true;
        [SerializeField] private bool _controlBlockRaycasts = true;
        
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.interactable = _initiallyFadedIn;
        }


        protected override void Setter(float alpha)
        {
            _canvasGroup.alpha = alpha;
            if (_controlInteractable) _canvasGroup.interactable = alpha.Approximate(0f);
            if (_controlBlockRaycasts) _canvasGroup.blocksRaycasts = alpha.Approximate(0f);
        }
    }
}
