using System;
using UnityEngine.UIElements;

namespace DualityGame.DialogSystem.UI
{
    public class DialogOption : VisualElement
    {
        private readonly Button _button;
        public event Action OnClicked;
        
        public new class UxmlFactory : UxmlFactory<DialogOption, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription textAttr = new()
            {
                name = "text", defaultValue = ""
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                if (ve is not DialogOption dialogOption) return;
                dialogOption.text = textAttr.GetValueFromBag(bag, cc);
            }
        }

        private string text
        {
            get => _button.text;
            set => _button.text = value;
        }

        public DialogOption() : this("", null)
        {
        }

        public void SelectOption() => OnClicked?.Invoke();

        public DialogOption(string text, Action onClicked)
        {
            _button = new Button() { text = text };
            _button.clicked += () => OnClicked?.Invoke();
            hierarchy.Add(_button);

            if (onClicked != null) OnClicked = onClicked;
        }
    }
}
