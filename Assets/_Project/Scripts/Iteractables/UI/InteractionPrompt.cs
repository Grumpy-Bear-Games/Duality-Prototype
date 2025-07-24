using System;
using UnityEngine.UIElements;

namespace DualityGame.Iteractables.UI
{

    [UxmlElement]
    public partial class InteractionPrompt : VisualElement
    {
        private IInteractable.InteractionType _interactionType;
        private bool _selected;

        private const string USSClassNameBase = "interaction-prompt";
        private const string KeyUssClassName = USSClassNameBase + "__icon";

        [UxmlAttribute]
        public IInteractable.InteractionType InteractionType
        {
            get => _interactionType;
            set
            {
                _interactionType = value;
                UpdateElement();
            }
        }

        [UxmlAttribute]
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                UpdateElement();
            }
        }
        
        
        private void UpdateElement()
        {
            foreach(var interactionType in Enum.GetValues(typeof(IInteractable.InteractionType)))
                RemoveFromClassList(interactionType.ToString().ToLower());

            if (!_selected) return;
            AddToClassList(_interactionType.ToString().ToLower());
        }

        public InteractionPrompt()
        {
            AddToClassList(USSClassNameBase);
            AddToClassList("initial");
            schedule.Execute(() => RemoveFromClassList("initial")).ExecuteLater(10);

            var icon = new VisualElement
            {
                name = "Icon",
            };
            icon.AddToClassList(KeyUssClassName);

            hierarchy.Add(icon);
            UpdateElement();
        }
    }
}
