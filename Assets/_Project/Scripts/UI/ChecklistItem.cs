
using Unity.Properties;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    [UxmlElement]
    public partial class ChecklistItem: VisualElement
    {
        private const string SuccessCheckMark = "✔";

        private readonly Label _tickLabel;
        private readonly Label _titleLabel;
        private bool _checked;
        private string _title;

        [UxmlAttribute]
        [CreateProperty]
        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                UpdateVisuals();
            }
        }

        [UxmlAttribute]
        [CreateProperty]
        public string Title
        {
            get => _title;
            set {
                _title = value;
                UpdateVisuals();
            }
    }

        private void UpdateVisuals()
        {
            _tickLabel.text = _checked ? SuccessCheckMark : string.Empty;
            _titleLabel.text = _checked ? $"<s>{_title}" : _title;
        }

        public ChecklistItem()
        {
            AddToClassList("checklist-item");

            _tickLabel = new Label();
            _tickLabel.AddToClassList("checklist-item__tick");
            hierarchy.Add(_tickLabel);
            
            _titleLabel = new Label();
            _titleLabel.AddToClassList("checklist-item__title");
            hierarchy.Add(_titleLabel);
            
            UpdateVisuals();
        }
    }
}
