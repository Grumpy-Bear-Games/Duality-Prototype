using System;
using System.Collections.Generic;
using System.Linq;
using DualityGame.Inventory;
using Games.GrumpyBear.Core.SaveSystem;
using NodeCanvas.DialogueTrees;
using Unity.Properties;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Serialization;

namespace DualityGame.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Duality/Quest", order = 0)]
    public class Quest : SerializableScriptableObject<Quest>
    {
        public static IEnumerable<Quest> AllQuests
        {
            get
            {
                FindAllInstances();
                return _instances.Values.OfType<Quest>();
            }
        }

        #region Events
        public static event Action<Quest> OnChange;
        public static event Action AfterStateRestored;
        public static event Action<Quest> OnBeginQuest;
        public static event Action<Quest> OnRevealQuest;
        public static event Action<Quest> OnCompleteQuest;
        #endregion

        #region Serialized fields
        [SerializeField, FormerlySerializedAs("<Title>k__BackingField")] private string _title;
        [SerializeField, FormerlySerializedAs("<Description>k__BackingField")] private string _description;
        [SerializeField, FormerlySerializedAs("<NPCVisibility>k__BackingField")] private QuestVisibility _visibility = QuestVisibility.ShowAutomaticallyWhenOngoing;
        [SerializeField] private Realm.Realm _stampRealm;

        [SerializeField, FormerlySerializedAs("<NPC>k__BackingField")] private ActorAsset _npc;
        [SerializeField, FormerlySerializedAs("<NPCVisibility>k__BackingField")] private QuestNPCVisibility _npcVisibility = new();

        [SerializeField, FormerlySerializedAs("<QuestItems>k__BackingField")] private List<QuestItem> _questItems;
        [SerializeField, FormerlySerializedAs("<CheckpointGroups>k__BackingField")] private List<CheckpointGroup>  _checkpointGroups;
        #endregion

        #region Private fields
        private QuestStatus  _status;
        #endregion

        #region Properties
        public string Title => _title;
        public string Description => _description;
        public QuestVisibility Visibility => _visibility;
        public Realm.Realm StampRealm => _stampRealm;
        public ActorAsset NPC => _npc;
        public QuestNPCVisibility NPCVisibility => _npcVisibility;
        public IReadOnlyList<QuestItem> QuestItems => _questItems;
        public IReadOnlyList<CheckpointGroup> CheckpointGroups => _checkpointGroups;
        public QuestStatus Status
        {
            get => _status;
            set
            {
                if (_status == value) return;
                var prev = _status;
                _status = value;
                switch ((_prev: prev, value))
                {
                    case (_, QuestStatus.Ongoing):
                        Started = DateTime.Now.Ticks;
                        IsVisible = _visibility == QuestVisibility.ShowAutomaticallyWhenOngoing;
                        OnBeginQuest?.Invoke(this);
                        break;

                    case (_, QuestStatus.NotStarted):
                        Started = 0;
                        IsVisible = false;
                        break;

                    case (QuestStatus.NotStarted, QuestStatus.Completed):
                        Started = DateTime.Now.Ticks;
                        IsVisible = true;
                        OnCompleteQuest?.Invoke(this);
                        break;

                    case (QuestStatus.Ongoing, QuestStatus.Completed):
                        IsVisible = true;
                        OnCompleteQuest?.Invoke(this);
                        break;

                }
                OnChange?.Invoke(this);
            }
        }
        public long Started { get; private set; }
        public bool IsVisible { get; private set; }
        #endregion

        #region Public Methods
        public void Begin() => Status = QuestStatus.Ongoing;

        public void Reveal()
        {
            if (_status != QuestStatus.Ongoing && IsVisible) return;
            IsVisible = true;
            OnRevealQuest?.Invoke(this);
            OnChange?.Invoke(this);
        }

        public void Complete() => Status = Quest.QuestStatus.Completed;
        #endregion

        #region Inner classes
        [Serializable]
        public class QuestItem
        {
            [SerializeField] private ItemType _itemType;
            [SerializeField] private QuestItemVisibility _visibility;
            [SerializeField] private List<Checkpoint> _checkpoints = new();

            [CreateProperty]
            public ItemType ItemType => _itemType;

            [CreateProperty]
            public QuestItemVisibility Visibility => _visibility;

            [CreateProperty]
            public List<Checkpoint> Checkpoints => _checkpoints;
        }


        [Serializable]
        public sealed class CheckpointGroup
        {
            [SerializeField] private List<Checkpoint> _checkpoints = new();

            public IReadOnlyList<Checkpoint> Checkpoints => _checkpoints;
        }

        [Serializable]
        public sealed class QuestNPCVisibility
        {
            [SerializeField] private NPCVisibilityCondition _visibility;
            [SerializeField] private List<Checkpoint> _checkpoints = new();

            public NPCVisibilityCondition Visibility => _visibility;
            public IReadOnlyList<Checkpoint> Checkpoints => _checkpoints;
        }

        #endregion

        #region Public enums
        public enum QuestStatus
        {
            NotStarted,
            Ongoing,
            Completed,
        }

        public enum NPCVisibilityCondition
        {
            AlwaysVisible,
            VisibleAfterCheckpoints,
            VisibleAfterComplete,
        }


        public enum QuestVisibility
        {
            ShowAutomaticallyWhenOngoing,
            ManualVisibility
        }

        public enum QuestItemVisibility
        {
            AlwaysVisible,
            VisibleAfterCheckpoints,
            VisibleWhenAcquired,
        }
        #endregion

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void ClearAll()
        {
            foreach (var quest in AllQuests)
            {
                quest._status = QuestStatus.NotStarted;
                quest.Started = 0;
                quest.IsVisible = false;
            }
            AfterStateRestored?.Invoke();
        }

        #if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }
        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode) ClearAll();
        }
        #endif

        #region ISaveableComponent
        [Serializable]
        private class SerializableQuestEntry
        {
            public readonly ObjectGuid QuestID;
            public readonly long Started;
            public readonly int Status;
            public readonly bool IsVisible;

            public SerializableQuestEntry(Quest quest)
            {
                QuestID = quest.ObjectGuid;
                Started = quest.Started;
                Status = (int)quest.Status;
                IsVisible = quest.IsVisible;
            }
        }

        public static object CaptureState() => AllQuests
            .Where(quest => quest.Status != Quest.QuestStatus.NotStarted)
            .Select(quest => new SerializableQuestEntry(quest))
            .ToList();

        public static void RestoreState(object state)
        {
            ClearAll();
            if (state is not List<SerializableQuestEntry> entries)
            {
                Debug.LogError($"Unexpected object type {state.GetType()}");
                return;
            }
            foreach (var entry in entries)
            {
                var quest = GetByGuid(entry.QuestID);
                quest._status =  (Quest.QuestStatus) entry.Status;
                quest.Started = entry.Started;
                quest.IsVisible = entry.IsVisible;
            }
            AfterStateRestored?.Invoke();
        }
        #endregion
    }
}
