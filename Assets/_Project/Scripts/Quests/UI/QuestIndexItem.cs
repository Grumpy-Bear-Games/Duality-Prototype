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
        
        private QuestLog.QuestEntry _questEntry;

        public QuestLog.QuestEntry QuestEntry
        {
            get => _questEntry;
            set
            {
                _questEntry = value;
                UpdateVisuals();
            }
        }

        private void UpdateVisuals()
        {
            if (_questEntry == null) return; 
            var isOngoing = _questEntry.State == QuestLog.QuestState.Ongoing;
            var title = _questEntry.Quest.TitleWithNPC;

            _questResolved.text = _questEntry.State switch
            {
                QuestLog.QuestState.Ongoing => "",
                QuestLog.QuestState.Succeeded => SuccessCheckMark,
                QuestLog.QuestState.Failed => FailCheckMark,
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
