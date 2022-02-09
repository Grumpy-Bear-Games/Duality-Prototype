using DualityGame.Core;
using TMPro;
using UnityEngine;

namespace DualityGame.Iteractables
{
    public class InteractionPrompt : MonoBehaviour
    {
        [SerializeField] private InteractController _controller;
        [SerializeField] private TMP_Text _promptText;
        [SerializeField] private CanvasGroup _promptUI;

        private IInteractable _interactable;

        private void OnEnable()
        {
            _controller.ClosestInteractable.Subscribe(OnClosestInteractableChange);
            PlayState.Current.Subscribe(EvaluatePrompt);
        }

        private void OnDisable()
        {
            _controller.ClosestInteractable.Unsubscribe(OnClosestInteractableChange);
            PlayState.Current.Unsubscribe(EvaluatePrompt);
        }

        private void OnClosestInteractableChange(IInteractable interactable)
        {
            _interactable = interactable;
            EvaluatePrompt(PlayState.Current.Value);
        }

        private void EvaluatePrompt(PlayState.State playState)
        {
            if (_interactable != null && playState == PlayState.State.Moving)
            {
                _promptText.text = _interactable.Prompt;
                _promptUI.alpha = 1f;
            } else {
                _promptText.text = "";
                _promptUI.alpha = 0f;
            }
        }

        private void Reset()
        {
            _promptText = GetComponentInChildren<TMP_Text>();
            _promptUI = GetComponentInChildren<CanvasGroup>();
            _controller = FindObjectOfType<InteractController>();
        }
    }
}
