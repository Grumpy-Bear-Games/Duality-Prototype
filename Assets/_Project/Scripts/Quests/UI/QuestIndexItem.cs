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
        
        private Quest.QuestState _questState;

        public Quest.QuestState QuestState
        {
            get => _questState;
            set
            {
                _questState = value;
                UpdateVisuals();
            }
        }

        private void UpdateVisuals()
        {
            if (_questState == null) return; 
            var isOngoing = _questState.Status == Quest.QuestStatus.Ongoing;
            var title = _questState.Quest.TitleWithNPC;

            _questResolved.text = _questState.Status switch
            {
                Quest.QuestStatus.Ongoing => "",
                Quest.QuestStatus.Succeeded => SuccessCheckMark,
                Quest.QuestStatus.Failed => FailCheckMark,
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
