using System;
using TMPro;
using UnityEngine;

namespace DualityGame.Iteractables
{
    public class InteractionPrompt : MonoBehaviour
    {
        [SerializeField] private InteractController _controller;
        [SerializeField] private TMP_Text _promptText;
        [SerializeField] private CanvasGroup _promptUI;

        private void OnEnable() => _controller.ClosestInteractable.Subscribe(OnClosestInteractableChange);
        private void OnDisable() => _controller.ClosestInteractable.Unsubscribe(OnClosestInteractableChange);

        private void OnClosestInteractableChange(IInteractable interactable)
        {
            if (interactable != null)
            {
                _promptText.text = interactable.Prompt;
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
