using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Quests.UI
{
    public class QuestSlot : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<QuestSlot> { }

        private readonly Label _titleLabel;
        private readonly Label _descriptionLabel;

        private Quest _quest;
        
        public Quest Quest
        {
            get => _quest;
            set
            {
                _quest = value;
                UpdateVisuals();
            }
        }

        private const string StyleResource = "QuestSlot";
        private const string USSClassNameBase = "quest-slot";
        private const string TitleUssClassName = USSClassNameBase + "__title";
        private const string DescriptionUssClassName = USSClassNameBase + "__description";


        private void UpdateVisuals()
        {
            if (_quest == null)
            {
                _titleLabel.text = "";
                _descriptionLabel.text = "";
            }
            else
            {
                _titleLabel.text = _quest.Title;
                _descriptionLabel.text = _quest.Description;
            }
        }
            
        public QuestSlot()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(StyleResource));
            
            AddToClassList(USSClassNameBase);
            
            _titleLabel = new Label
            {
                name = "Title"
            };
            _titleLabel.AddToClassList(TitleUssClassName);
            hierarchy.Add(_titleLabel);

            _descriptionLabel = new Label
            {
                name = "Description"
            };
            _descriptionLabel.AddToClassList(DescriptionUssClassName);
            hierarchy.Add(_descriptionLabel);

            #if UNITY_EDITOR
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                _titleLabel.text = "Quest title";
                _descriptionLabel.text = "This is the quest description";
            }
            else
            {
                UpdateVisuals();
            }
            #endif
        }
    }
}
