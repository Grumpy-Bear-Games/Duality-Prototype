﻿using System;
using UnityEngine.UIElements;

namespace DualityGame.Iteractables.UI
{
    public class InteractionPrompt : VisualElement
    {
        private IInteractable.InteractionType _interactionType;
        private bool _selected;

        public new class UxmlFactory : UxmlFactory<InteractionPrompt, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlEnumAttributeDescription<IInteractable.InteractionType> _type = new()
            {
                name = "interaction-type", defaultValue = IInteractable.InteractionType.Touch
            };

            private readonly UxmlBoolAttributeDescription _selected = new()
            {
                name = "selected", defaultValue = false
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                if (ve is not InteractionPrompt interactionPrompt) return;
                interactionPrompt.InteractionType = _type.GetValueFromBag(bag, cc);
                interactionPrompt.Selected = _selected.GetValueFromBag(bag, cc);
            }
        }

        private const string USSClassNameBase = "interaction-prompt";
        private const string KeyUssClassName = USSClassNameBase + "__icon";

        public IInteractable.InteractionType InteractionType
        {
            get => _interactionType;
            set
            {
                _interactionType = value;
                UpdateElement();
            }
        }

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
