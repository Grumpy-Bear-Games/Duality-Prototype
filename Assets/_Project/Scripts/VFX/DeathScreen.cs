using System;
using System.Collections;
using DualityGame.Player;
using DualityGame.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.VFX
{
    [RequireComponent(typeof(UIDocument))]
    public class DeathScreen : ScreenFaderProviderBase
    {
        private Label _label;
        private VisualElement _root;
        
        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _label = _root.Q<Label>();
            CauseOfDeath.OnDeath += UpdateCauseOfDeathLabel;
            Hide();
        }

        private void OnDestroy() => CauseOfDeath.OnDeath -= UpdateCauseOfDeathLabel;

        private void UpdateCauseOfDeathLabel(CauseOfDeath causeOfDeath) => _label.text = causeOfDeath.Description;

        public override IEnumerator Execute(ScreenFader.Direction direction)
        {
            using var transitionMonitor = new TransitionMonitor(_root);

            switch (direction)
            {
                case ScreenFader.Direction.FadeIn:
                    Hide();
                    break;
                case ScreenFader.Direction.FadeOut:
                    Show();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
            yield return transitionMonitor.WaitUntilDone();
        }
        private void Hide() => _root.AddToClassList("FadeOut");
        private void Show() => _root.RemoveFromClassList("FadeOut");
    }
}
