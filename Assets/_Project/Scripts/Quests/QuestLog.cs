using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DualityGame.Quests
{
    [CreateAssetMenu(fileName = "Quest Log", menuName = "Duality/Quest Log", order = 0)]
    public class QuestLog : ScriptableObject
    {
        private readonly Dictionary<Quest, QuestEntry> _quests = new();
        
        public void Add(Quest quest)
        {
            if (_quests.ContainsKey(quest)) return;
            var questStates = new QuestEntry(quest);
            _quests.Add(quest, questStates);
        }

        public bool Contains(Quest quest) => _quests.ContainsKey(quest);

        public QuestEntry GetEntry(Quest quest) => !_quests.ContainsKey(quest) ? null : _quests[quest];

        public class QuestEntry
        {
            public readonly Quest Quest;
            public readonly long Started;
            public QuestState State;
            public bool Visible;
            
            public QuestEntry(Quest quest)
            {
                Quest = quest;
                Started = DateTime.Now.Ticks;
                State = QuestState.Ongoing;
                Visible = quest.Visibility == Quest.QuestVisibility.ShowAutomaticallyWhenOngoing;
            }
        }
        
        #region Public enums
        public enum QuestState
        {
            Inactive,
            Ongoing,
            Succeeded,
            Failed,
        }
        #endregion
        
        #region Life-cycle management
        private void OnEnable()
        {
            #if UNITY_EDITOR
            if (!EditorApplication.isPlayingOrWillChangePlaymode) return;
            _quests.Clear();
            #endif
        }
        #endregion
    }
}
