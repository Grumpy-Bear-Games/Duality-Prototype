using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Iteractables.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class InteractionUI : MonoBehaviour
    {
        [SerializeField] private InteractableObservable _closestInteractable;
        [SerializeField] private float _radius = 3f;
        [SerializeField] private float _viewRadius = 6f;
        [SerializeField] private Transform _origin;
        [SerializeField] private StyleSheet _styleSheet;

        private VisualElement _root;

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _root.styleSheets.Add(_styleSheet);
        }

        private void HidePrompt() => _root.style.display = DisplayStyle.None;

        private void ShowPrompt() => _root.style.display = DisplayStyle.Flex;

        private void OnEnable() => ShowPrompt();

        private void OnDisable() => HidePrompt();

        private readonly Dictionary<IInteractable, UI.InteractionPrompt> _inRange = new();

        private void Remove(IInteractable interactable)
        {
            if (!_inRange.TryGetValue(interactable, out var prompt)) return;
            prompt.RemoveFromHierarchy();
            _inRange.Remove(interactable);
        }

        private void Add(IInteractable interactable)
        {
            if (!_inRange.TryGetValue(interactable, out var prompt))
            {
                prompt = new InteractionPrompt();
                _root.Add(prompt);
                _inRange[interactable] = prompt;
            }
            prompt.InteractionType = interactable.Type;
        }

        private void Update()
        {
            IInteractable closest = null;
            var closestDistance = Mathf.Infinity;
            foreach (var interactable in Interactable.Interactables)
            {
                if (!interactable.Enabled)
                {
                    Remove(interactable);
                    continue;
                }
                var distance = Vector3.Distance(_origin.position, interactable.Position);
                if (distance > _viewRadius)
                {
                    Remove(interactable);
                    continue;
                }

                Add(interactable);
                if (closest != null && distance > closestDistance) continue;
                closest = interactable;
                closestDistance = distance;
            }

            var toBeDeleted = new List<IInteractable>();
            foreach (var (interactable, prompt) in _inRange)
            {
                if (!Interactable.Interactables.Contains(interactable))
                {
                    toBeDeleted.Add(interactable);
                    continue;
                }
                
                var promptPosition = RuntimePanelUtils.CameraTransformWorldToPanel(_root.panel, interactable.PromptPosition, Camera.main);
                prompt.style.top = promptPosition.y;
                prompt.style.left = promptPosition.x;
                prompt.Selected = (closest == interactable && closestDistance < _radius);
            }
            toBeDeleted.ForEach(Remove);
            _closestInteractable.Set(closestDistance < _radius ? closest : null);
        }
    }
}
