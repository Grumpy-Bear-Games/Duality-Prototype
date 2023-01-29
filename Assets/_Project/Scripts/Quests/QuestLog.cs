using System;
using System.Collections.Generic;
using System.Linq;
using DualityGame.SaveSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DualityGame.Quests
{
    
    [CreateAssetMenu(fileName = "Quest Log", menuName = "Duality/Quest Log", order = 0)]
    public class QuestLog : ScriptableObject, ISaveableComponent
    {
        public event Action OnChange;
        
        private readonly Dictionary<Quest, QuestEntry> _quests = new();
        
        public void Add(Quest quest)
        {
            if (_quests.ContainsKey(quest)) return;
            var questStates = new QuestEntry(quest);
            _quests.Add(quest, questStates);
        }

        public bool Contains(Quest quest) => _quests.ContainsKey(quest);

        public void RevealQuest(Quest quest)
        {
            _quests.TryGetValue(quest, out var questEntry);
            if (questEntry == null) return;  // TODO: Maybe an exception instead?
            if (questEntry.Visible) return;
            questEntry.Visible = true;
            OnChange?.Invoke();
        }

        public void SucceedQuest(Quest quest) => SetQuestState(quest: quest, questState: QuestState.Succeeded);

        public void FailQuest(Quest quest) => SetQuestState(quest: quest, questState: QuestState.Failed);

        private void SetQuestState(Quest quest, QuestState questState)
        {
            _quests.TryGetValue(quest, out var questEntry);
            if (questEntry == null) return;  // TODO: Maybe an exception instead?
            questEntry.State = questState; // TODO: Check previous state
            OnChange?.Invoke();
        }
        
        public QuestEntry GetEntry(Quest quest) => !_quests.ContainsKey(quest) ? null : _quests[quest];

        public class QuestEntry
        {
            public readonly Quest Quest;
            public readonly long Started;
            public QuestState State;
            public bool Visible;
            
            public QuestEntry(Quest quest) : this(
                quest: quest,
                started: DateTime.Now.Ticks,
                state: QuestState.Ongoing,
                visible: quest.Visibility == Quest.QuestVisibility.ShowAutomaticallyWhenOngoing) { }

            public QuestEntry(Quest quest, long started, QuestState state, bool visible)
            {
                Quest = quest;
                Started = started;
                State = state;
                Visible = visible;
            }
        }
        
        #region Public enums
        public enum QuestState
        {
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

        #region ISaveableComponent
        
        [Serializable]
        private class SerializableQuestEntry
        {
            public readonly string QuestID;
            public readonly long Started;
            public readonly int State;
            public readonly bool Visible;
            
            public SerializableQuestEntry(QuestEntry questEntry)
            {
                QuestID = questEntry.Quest.GUID;
                Started = questEntry.Started;
                State = (int)questEntry.State;
                Visible = questEntry.Visible;
            }

            public QuestEntry ToQuestEntry() =>
                new(
                    quest: Quest.GetByGUID(QuestID),
                    started: Started,
                    state: (QuestState)State,
                    visible: Visible
                );
        }

        object ISaveableComponent.CaptureState() => _quests.Values
            .Select(entry => new SerializableQuestEntry(entry))
            .ToList();

        void ISaveableComponent.RestoreState(object state)
        {
            var entries = (List<SerializableQuestEntry>)state;
            _quests.Clear();
            foreach (var entry in entries.Select(serializableQuestEntry => serializableQuestEntry.ToQuestEntry()))
            {
                _quests[entry.Quest] = entry;
            }
            OnChange?.Invoke();
        }
        #endregion
    }
}
