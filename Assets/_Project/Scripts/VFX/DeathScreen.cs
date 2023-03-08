using DualityGame.Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.VFX
{
    [RequireComponent(typeof(UIDocument))]
    public class DeathScreen : UIToolkitScreenFaderBase
    {
        private Label _label;

        protected override void Awake()
        {
            base.Awake();
            _root = _root.Q<VisualElement>("DeathScreen");
            _label = _root.Q<Label>("DeathMessage");
            CauseOfDeath.OnDeath += UpdateCauseOfDeathLabel;
            FadeIn();
        }

        private void OnDestroy() => CauseOfDeath.OnDeath -= UpdateCauseOfDeathLabel;

        private void UpdateCauseOfDeathLabel(CauseOfDeath causeOfDeath) => _label.text = causeOfDeath.Description;

        protected override void FadeIn() => _root.RemoveFromClassList("Shown");

        protected override void FadeOut() => _root.AddToClassList("Shown");
    }
}
