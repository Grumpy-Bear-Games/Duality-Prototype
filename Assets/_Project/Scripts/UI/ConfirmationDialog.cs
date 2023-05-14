using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace DualityGame.UI
{
    public class ConfirmationDialog : VisualElement
    {
        private readonly Button _confirmButton;
        private readonly Button _cancelButton;
        private readonly FocusOnShow _focusOnShow;

        public event Action OnConfirm;
        
        private const string StyleResource = "ConfirmationDialog";
        private const string USSClassNameBase = "confirmation-dialog";
        private const string FrameUssClassName = USSClassNameBase + "__frame";
        private const string HeaderUssClassName = USSClassNameBase + "__header";
        private const string ButtonsUssClassName = USSClassNameBase + "__buttons";
        private const string ConfirmButtonUssClassName = USSClassNameBase + "__confirm-button";
        private const string CancelButtonUssClassName = USSClassNameBase + "__cancel-button";

        public static void ShowConfirm(VisualElement root,
            string header, string confirmLabel, string cancelLabel = "Back",
            Action onConfirm = null, FocusOnShow focusOnShow = FocusOnShow.Cancel)
        {
            var confirm = new ConfirmationDialog(header, confirmLabel, cancelLabel, focusOnShow);
            confirm.OnConfirm = onConfirm;
            root.Add(confirm);
            confirm.Show();
        }

        private ConfirmationDialog(string header, string confirmLabel, string cancelLabel, FocusOnShow focusOnShow)
        {
            styleSheets.Add(Resources.Load<StyleSheet>(StyleResource));
            AddToClassList(USSClassNameBase);
            
            var frame = new VisualElement() { name = "Frame" } ;
            frame.AddToClassList(FrameUssClassName);
            hierarchy.Add(frame);
            
            RegisterCallback<PointerDownEvent>(evt =>
            {
                evt.StopImmediatePropagation();
                evt.PreventDefault();
            });
            
            frame.RegisterCallback<NavigationCancelEvent>(_ => Close());

            var headerLabel = new Label() { name = "Header", text = header};
            headerLabel.AddToClassList(HeaderUssClassName);
            frame.Add(headerLabel);

            var buttons = new VisualElement();
            buttons.AddToClassList(ButtonsUssClassName);
            frame.Add(buttons);
            
            _confirmButton = new Button() { name = "ConfirmButton", text = confirmLabel} ;
            _confirmButton.AddToClassList(ConfirmButtonUssClassName);
            _confirmButton.clicked += OnConfirmButtonClicked;
            buttons.Add(_confirmButton);
            
            _cancelButton = new Button() { name = "CancelButton", text = cancelLabel};
            _cancelButton.AddToClassList(CancelButtonUssClassName);
            _cancelButton.clicked += Close;
            buttons.Add(_cancelButton);
            
            _confirmButton.RegisterCallback<NavigationMoveEvent>(evt =>
            {
                switch (evt.direction)
                {
                    case NavigationMoveEvent.Direction.Right:
                    case NavigationMoveEvent.Direction.Left:
                        _cancelButton.Focus();
                        break;
                }
                evt.PreventDefault();
            });
            
            _cancelButton.RegisterCallback<NavigationMoveEvent>(evt =>
            {
                switch (evt.direction)
                {
                    case NavigationMoveEvent.Direction.Right:
                    case NavigationMoveEvent.Direction.Left:
                        _confirmButton.Focus();
                        break;
                }
                evt.PreventDefault();
            });
            
            _focusOnShow = focusOnShow;
        }

        private void Show()
        {
            FocusHelper.Push();
            BringToFront();
            
            style.display = DisplayStyle.Flex;
            switch (_focusOnShow)
            {
                case FocusOnShow.Confirm:
                    _confirmButton.Focus();
                    break;
                case FocusOnShow.Cancel:
                    _cancelButton.Focus();
                    break;
            }
        }

        private void Close()
        {
            parent.Remove(this);
            FocusHelper.Pop();
        }

        private void OnConfirmButtonClicked()
        {
            Close();
            OnConfirm?.Invoke();
        }

        public enum FocusOnShow
        {
            Confirm,
            Cancel
        }
    }
}
