using System;
using System.Collections.Generic;
using System.Linq;
using Games.GrumpyBear.Core.SaveSystem;
using NodeCanvas.DialogueTrees;
using Unity.Properties;
using UnityEngine;

namespace DualityGame.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Duality/Quest", order = 0)]
    public class Quest : SerializableScriptableObject<Quest>
    {
        private static readonly Dictionary<Quest, QuestState> _questStates = new();
        public static IEnumerable<Quest> VisibleQuests => _questStates.Values.Where(questState => questState.Visible).Select(questState => questState.Quest);

        public static event Action<Quest> OnChange;
        public static event Action AfterStateRestored;


        #region Serialized fields
        [field: SerializeField] public string Title { get; private set; }
        [field: Multiline][field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public ActorAsset NPC { get; private set; }
        [field: SerializeField] public Realm.Realm Realm { get; private set; }
        [field: SerializeField] public QuestVisibility Visibility { get; private set; } =
            QuestVisibility.ShowAutomaticallyWhenOngoing;
        #endregion

        [CreateProperty]
        public string TitleWithNPC => NPC != null ? $"{Title} ({NPC.Name})" : Title;

        [CreateProperty]
        public bool IsVisible => _questStates.TryGetValue(this, out var questState) && questState.Visible;

        [CreateProperty]
        public long Started => _questStates.TryGetValue(this, out var questState) ? questState.Started : 0;

        [CreateProperty]
        public QuestStatus Status => _questStates.TryGetValue(this, out var questState) ? questState.Status : QuestStatus.NotStarted;

        public void Begin()
        {
            if (_questStates.ContainsKey(this)) return;
            var questState = new QuestState(this);
            _questStates.Add(this, questState);
            if (Visibility == QuestVisibility.ShowAutomaticallyWhenOngoing) {
                Notifications.Notifications.Add(NPC.PortraitByMood(Mood.Neutral), $"You started a new quest: {Title}");
            }
            OnChange?.Invoke(this);
        }

        public void Reveal()
        {
            _questStates.TryGetValue(this, out var questState);
            if (questState == null) return;  // TODO: Maybe an exception instead?
            if (questState.Visible) return;
            questState.Visible = true;
            Notifications.Notifications.Add(NPC.PortraitByMood(Mood.Neutral), $"Quest Revealed: {Title}");
            OnChange?.Invoke(this);
        }

        public void Succeed()
        {
            Complete(QuestStatus.Succeeded);
            Notifications.Notifications.Add(NPC.PortraitByMood(Mood.Happy),$"You completed a quest: {Title}");
        }

        public void Fail()
        {
            Complete(Quest.QuestStatus.Failed);
            Notifications.Notifications.Add(NPC.PortraitByMood(Mood.Sad),$"You failed a quest: {Title}");
        }

        private void Complete(QuestStatus status)
        {
            if (!_questStates.TryGetValue(this, out var questState))
            {
                questState = new QuestState(this);
                _questStates.Add(this, questState);
            }

            if (questState.Status == status) return;
            questState.Visible = true;
            questState.Status = status;
            OnChange?.Invoke(this);
        }


        #region Public enums
        public enum QuestVisibility
        {
            ShowAutomaticallyWhenOngoing,
            ManualVisibility
        }

        public enum QuestStatus
        {
            NotStarted,
            Ongoing,
            Succeeded,
            Failed,
        }
        #endregion

        private class QuestState
        {
            public readonly Quest Quest;
            public readonly long Started;
            public QuestStatus Status;
            public bool Visible;

            public QuestState(Quest quest) : this(
                quest: quest,
                started: DateTime.Now.Ticks,
                status: QuestStatus.Ongoing,
                visible: quest.Visibility == QuestVisibility.ShowAutomaticallyWhenOngoing) { }

            public QuestState(Quest quest, long started, QuestStatus status, bool visible)
            {
                Quest = quest;
                Started = started;
                Status = status;
                Visible = visible;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void ClearAll()
        {
            _questStates.Clear();
            AfterStateRestored?.Invoke();
        }

        #region ISaveableComponent
        [Serializable]
        private class SerializableQuestEntry
        {
            public readonly ObjectGuid QuestID;
            public readonly long Started;
            public readonly int Status;
            public readonly bool Visible;

            public SerializableQuestEntry(Quest.QuestState questState)
            {
                QuestID = questState.Quest.ObjectGuid;
                Started = questState.Started;
                Status = (int)questState.Status;
                Visible = questState.Visible;
            }

            public QuestState ToQuestEntry() =>
                new(
                    quest: GetByGuid(QuestID),
                    started: Started,
                    status: (QuestStatus)Status,
                    visible: Visible
                );
        }

        public static object CaptureState() => _questStates.Values
            .Select(entry => new SerializableQuestEntry(entry))
            .ToList();

        public static void RestoreState(object state)
        {
            var entries = (List<SerializableQuestEntry>)state;
            _questStates.Clear();
            foreach (var entry in entries.Select(serializableQuestEntry => serializableQuestEntry.ToQuestEntry()))
            {
                _questStates[entry.Quest] = entry;
            }
            AfterStateRestored?.Invoke();
        }
        #endregion

    }
}
