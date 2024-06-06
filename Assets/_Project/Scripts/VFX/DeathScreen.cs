using System.Collections;
using DualityGame.Core;
using DualityGame.Player;
using DualityGame.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.VFX
{
    [RequireComponent(typeof(UIDocument))]
    public class DeathScreen: MonoBehaviour
    {
        private VisualElement _deathScreen;
        private Label _causeOfDeathLabel;

        private void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _deathScreen = root.Q<VisualElement>("DeathScreen");
            _causeOfDeathLabel = root.Q<Label>("DeathMessage");
            FadeIn();
        }

        private void OnEnable()
        {
            DeathController.OnPlayerDied += OnPlayerDied;
            DeathController.AfterRespawn += AfterRespawn;
        }

        private void OnDisable()
        {
            DeathController.OnPlayerDied -= OnPlayerDied;
            DeathController.AfterRespawn -= AfterRespawn;
        }

        #region OnPlayerDied
        private void OnPlayerDied(CauseOfDeath causeOfDeath, WaitForCompletion wfc) => StartCoroutine(OnPlayerDied_CO(causeOfDeath, wfc));

        private IEnumerator OnPlayerDied_CO(CauseOfDeath causeOfDeath, WaitForCompletion wfc)
        {
            using var trigger = wfc.CreateCompletionTrigger();
            using var transitionMonitor = new TransitionMonitor(_deathScreen);
            _causeOfDeathLabel.text = causeOfDeath.Description;
            _deathScreen.style.display = DisplayStyle.Flex;
            FadeOut();
            yield return transitionMonitor.WaitUntilDone();
        }
        #endregion

        #region AfterRespawn
        private void AfterRespawn(CauseOfDeath causeOfDeath, WaitForCompletion wfc) => StartCoroutine(AfterRespawn_CO(causeOfDeath, wfc));

        private IEnumerator AfterRespawn_CO(CauseOfDeath causeOfDeath, WaitForCompletion wfc)
        {
            using var trigger = wfc.CreateCompletionTrigger();
            using var transitionMonitor = new TransitionMonitor(_deathScreen);
            FadeIn();
            yield return transitionMonitor.WaitUntilDone();
        }
        #endregion

        private void FadeIn() => _deathScreen.RemoveFromClassList("Shown");

        private void FadeOut() => _deathScreen.AddToClassList("Shown");
    }
}
