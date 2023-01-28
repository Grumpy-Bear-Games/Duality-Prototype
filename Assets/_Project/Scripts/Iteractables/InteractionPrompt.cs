using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Iteractables
{
    public class InteractionPrompt : MonoBehaviour
    {
        [SerializeField] private InteractableObservable _closestInteractable;

        private IInteractable _interactable;
        private UIDocument _uiDocument;
        private VisualElement _prompt;
        private Label _promptLabel;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _prompt = _uiDocument.rootVisualElement.Q<VisualElement>("Prompt");
            _promptLabel = _prompt.Q<Label>("Label");
        }

        private void HidePrompt() => _prompt.style.display = DisplayStyle.None;

        private void ShowPrompt() => _prompt.style.display = DisplayStyle.Flex;

        private void OnEnable() => _closestInteractable.Subscribe(OnClosestInteractableChange);

        private void OnDisable()
        {
            _closestInteractable.Unsubscribe(OnClosestInteractableChange);
            HidePrompt();
        }

        private void LateUpdate()
        {
            if (_interactable == null) return;

            var promptPosition = RuntimePanelUtils.CameraTransformWorldToPanel(_prompt.panel, _interactable.PromptPosition, Camera.main);
            _prompt.style.top = promptPosition.y;
            _prompt.style.left = promptPosition.x;
        }

        private void OnClosestInteractableChange(IInteractable interactable)
        {
            _interactable = interactable;
            EvaluatePrompt();
        }

        private void EvaluatePrompt()
        {
            if (_interactable != null && !string.IsNullOrEmpty(_interactable.Prompt))
            {
                _promptLabel.text = _interactable.Prompt;
                ShowPrompt();
            } else {
                HidePrompt();
            }
        }
    }
}
