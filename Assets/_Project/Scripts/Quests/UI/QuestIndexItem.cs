#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine.UIElements;

namespace DualityGame.Quests.UI
{
    public class QuestIndexItem: VisualElement
    {
        private const string SuccessCheckMark = "✔";
        private const string FailCheckMark = "X";
        
        private readonly Label _questResolved;
        private readonly Label _questTitle;

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
        
        private void UpdateVisuals()
        {
            if (_quest == null) return;
            var isOngoing = _quest.Status == Quest.QuestStatus.Ongoing;
            var title = _quest.TitleWithNPC;

            _questResolved.text = _quest.Status switch
            {
                Quest.QuestStatus.Ongoing => "",
                Quest.QuestStatus.Succeeded => SuccessCheckMark,
                Quest.QuestStatus.Failed => FailCheckMark,
                Quest.QuestStatus.NotStarted => throw new InvalidOperationException("Quest is not started"),
                _ => throw new ArgumentOutOfRangeException()
            };
            _questTitle.text = isOngoing ? title : $"<s>{title}";
        }

        public QuestIndexItem()
        {
            style.flexDirection = FlexDirection.Row;

            _questResolved = new Label
            {
                name = "ResolvedCheckBox"
            };
            hierarchy.Add(_questResolved);
            
            _questTitle = new Label
            {
                name = "QuestTitle"
            };
            hierarchy.Add(_questTitle);
            
            UpdateVisuals();
        }
    }
}
