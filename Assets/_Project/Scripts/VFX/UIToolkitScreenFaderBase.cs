using System;
using System.Collections;
using DualityGame.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.VFX
{
    [RequireComponent(typeof(UIDocument))]
    public abstract class UIToolkitScreenFaderBase : ScreenFaderProviderBase
    {
        public const string FadeInClass = "FadeIn"; 
        protected VisualElement _root;
        
        protected virtual void Awake() => _root = GetComponent<UIDocument>().rootVisualElement;

        public override IEnumerator Execute(ScreenFader.Direction direction)
        {
            using var transitionMonitor = new TransitionMonitor(_root);

            switch (direction)
            {
                case ScreenFader.Direction.FadeIn:
                    FadeIn();
                    break;
                case ScreenFader.Direction.FadeOut:
                    FadeOut();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
            yield return transitionMonitor.WaitUntilDone();
        }

        protected void FadeIn() => _root.AddToClassList(FadeInClass);
        protected void FadeOut() => _root.RemoveFromClassList(FadeInClass);
    }
}
