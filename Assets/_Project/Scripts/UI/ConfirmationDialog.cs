using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    public class ConfirmationDialog : VisualElement
    {
        private readonly Label _headerLabel;
        private readonly Button _confirmButton;
        private readonly Button _cancelButton;

        public new class UxmlFactory : UxmlFactory<ConfirmationDialog, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription headerAttr = new()
            {
                name = "header", defaultValue = "Confirm?"
            };

            private readonly UxmlStringAttributeDescription confirmButtonAttr = new()
            {
                name = "confirm-button", defaultValue = "Confirm"
            };

            private readonly UxmlStringAttributeDescription cancelButtonAttr = new()
            {
                name = "cancel-button", defaultValue = "Cancel"
            };

            private readonly UxmlEnumAttributeDescription<AutoHide> autoHideAttr = new()
            {
                name = "auto-hide", defaultValue = AutoHide.Never
            };


            private readonly UxmlEnumAttributeDescription<FocusOnShow> focusOnShowAttr = new()
            {
                name = "focus-on-show", defaultValue = FocusOnShow.Confirm
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var confirmDialog = (ve as ConfirmationDialog); 
                
                confirmDialog.Header = headerAttr.GetValueFromBag(bag, cc);
                confirmDialog.ConfirmButtonText = confirmButtonAttr.GetValueFromBag(bag, cc);
                confirmDialog.CancelButtonText = cancelButtonAttr.GetValueFromBag(bag, cc);
                confirmDialog._autoHide = autoHideAttr.GetValueFromBag(bag, cc);
                confirmDialog._focusOnShowOnShow = focusOnShowAttr.GetValueFromBag(bag, cc);
            }
        }

        public string Header
        {
            get => _headerLabel.text;
            set => _headerLabel.text = value;
        }

        public string ConfirmButtonText
        {
            get => _confirmButton.text;
            set => _confirmButton.text = value;
        }

        public string CancelButtonText
        {
            get => _cancelButton.text;
            set => _cancelButton.text = value;
        }

        private AutoHide _autoHide;
        private FocusOnShow _focusOnShowOnShow;

        public event Action OnConfirm;
        public event Action OnCancel;
        
        private const string StyleResource = "ConfirmationDialog";
        private const string USSClassNameBase = "confirmation-dialog";
        private const string FrameUssClassName = USSClassNameBase + "__frame";
        private const string HeaderUssClassName = USSClassNameBase + "__header";
        private const string ButtonsUssClassName = USSClassNameBase + "__buttons";
        private const string ConfirmButtonUssClassName = USSClassNameBase + "__confirm-button";
        private const string CancelButtonUssClassName = USSClassNameBase + "__cancel-button";

        public ConfirmationDialog()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(StyleResource));
            AddToClassList(USSClassNameBase);
            
            var frame = new VisualElement() { name = "Frame" } ;
            frame.AddToClassList(FrameUssClassName);
            hierarchy.Add(frame);

            _headerLabel = new Label() { name = "Header" };
            _headerLabel.AddToClassList(HeaderUssClassName);
            frame.Add(_headerLabel);

            var buttons = new VisualElement();
            buttons.AddToClassList(ButtonsUssClassName);
            frame.Add(buttons);
            
            _confirmButton = new Button() { name = "ConfirmButton" } ;
            _confirmButton.AddToClassList(ConfirmButtonUssClassName);
            _confirmButton.clicked += OnConfirmButtonClicked;
            buttons.Add(_confirmButton);
            
            _cancelButton = new Button() { name = "CancelButton" };
            _cancelButton.AddToClassList(CancelButtonUssClassName);
            _cancelButton.clicked += OnCancelButtonClicked;
            buttons.Add(_cancelButton);
        }

        public void Show()
        {
            style.display = DisplayStyle.Flex;
            switch (_focusOnShowOnShow)
            {
                case FocusOnShow.Confirm:
                    _confirmButton.Focus();
                    break;
                case FocusOnShow.Cancel:
                    _cancelButton.Focus();
                    break;
            }
        }

        public void Hide() => style.display = DisplayStyle.None;

        private void OnCancelButtonClicked()
        {
            switch (_autoHide)
            {
                case AutoHide.OnCancel:
                case AutoHide.Always:
                    Hide();
                    break;
            }
            OnCancel?.Invoke();
        }

        private void OnConfirmButtonClicked()
        {
            switch (_autoHide)
            {
                case AutoHide.Always:
                    Hide();
                    break;
            }
            OnConfirm?.Invoke();
        }

        public enum AutoHide
        {
            Never,
            OnCancel,
            Always
        }

        public enum FocusOnShow
        {
            None,
            Confirm,
            Cancel
        }
    }
}
