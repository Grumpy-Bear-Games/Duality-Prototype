using DualityGame.Core;
using Games.GrumpyBear.LevelManagement;
using TMPro;
using UnityEngine;

namespace DualityGame.Iteractables
{
    public class InteractionPrompt : MonoBehaviour
    {
        [SerializeField] private TMP_Text _promptText;
        [SerializeField] private CanvasGroup _promptUI;
        [SerializeField] private InteractableObservable _closestInteractable;

        private Camera _camera;
        private IInteractable _interactable;

        private void OnEnable()
        {
            _closestInteractable.Subscribe(OnClosestInteractableChange);
            PlayState.Current.Subscribe(EvaluatePrompt);
            LocationManager.OnLocationChanged += OnLocationChanged;
        }

        private void OnDisable()
        {
            _closestInteractable.Unsubscribe(OnClosestInteractableChange);
            PlayState.Current.Unsubscribe(EvaluatePrompt);
            LocationManager.OnLocationChanged -= OnLocationChanged;
        }

        private void Update()
        {
            if (_interactable == null) return;
            var promptPosition = _camera.WorldToScreenPoint(_interactable.PromptPosition);
            transform.position = promptPosition;
        }

        private void OnLocationChanged(Location _) => _camera = Camera.main;

        private void OnClosestInteractableChange(IInteractable interactable)
        {
            _interactable = interactable;
            EvaluatePrompt(PlayState.Current.Value);
        }

        private void EvaluatePrompt(PlayState.State playState)
        {
            if (_interactable != null && playState == PlayState.State.Moving && !string.IsNullOrEmpty(_interactable.Prompt))
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
        }
    }
}
