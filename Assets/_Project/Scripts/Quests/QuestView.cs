using System;
using System.Collections.Generic;
using System.Linq;
using DualityGame.Inventory;
using NodeCanvas.DialogueTrees;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Quests
{
    [GeneratePropertyBag]
    public class QuestView : IDisposable, IDataSourceViewHashProvider, IComparable<QuestView>
    {
        private readonly Inventory.Inventory _inventory;
        private readonly Quest _quest;
        private long _version;
        private List<Checkpoint> _visibleCheckpoints = new();
        private List<QuestItemView> _questItems = new();


        public class QuestItemView
        {
            private readonly Quest.QuestItem _questItem;

            [CreateProperty]
            public ItemType ItemType => _questItem.ItemType;

            [CreateProperty]
            public int Amount { get; }

            [CreateProperty]
            public bool HasInInventory => Amount > 0;

            [CreateProperty]
            public bool HasMultipleInInventory => Amount > 1;

            public Quest.QuestItemVisibility Visibility => _questItem.Visibility;

            public IReadOnlyList<Checkpoint> Checkpoints => _questItem.Checkpoints;

            public QuestItemView(Quest.QuestItem questItem, int amount)
            {
                _questItem = questItem;
                Amount = amount;
            }
        }

        long IDataSourceViewHashProvider.GetViewHashCode() => _version;
        int IComparable<QuestView>.CompareTo(QuestView other) => (int)(_quest.Started - other._quest.Started);

        #region Properties
        [CreateProperty]
        public string Title => _quest.Title;

        [CreateProperty]
        public string TitleWithNPC => _quest.NPC != null && IsNPCVisible ? $"{Title} ({NPC.Name})" : Title;

        [CreateProperty]
        public string Description => _quest.Description;

        [CreateProperty]
        public Quest.QuestStatus Status => _quest.Status;

        [CreateProperty]
        public ActorAsset NPC => _quest.NPC;

        [CreateProperty]
        public Sprite NPCPortrait => (_quest.NPC != null && IsNPCVisible)
            ? _quest.NPC.PortraitByMood(Status == Quest.QuestStatus.Completed ? Mood.Happy : Mood.Neutral)
            : null;

        [CreateProperty]
        public Sprite Stamp => HasCompleted ? _quest.StampRealm?.Stamp : null;

        [CreateProperty]
        public bool HasCompleted => Status == Quest.QuestStatus.Completed;

        [CreateProperty]
        public IReadOnlyList<QuestItemView> QuestItems => _questItems;

        [CreateProperty]
        public IReadOnlyList<Checkpoint> VisibleCheckpoints => _visibleCheckpoints;
        #endregion


        #region Private methods and properties
        private bool IsNPCVisible => _quest.NPCVisibility.Visibility switch
        {
            Quest.NPCVisibilityCondition.AlwaysVisible => true,
            Quest.NPCVisibilityCondition.VisibleAfterCheckpoints => _quest.NPCVisibility.Checkpoints.All(c => c.Reached),
            Quest.NPCVisibilityCondition.VisibleAfterComplete => Status == Quest.QuestStatus.Completed,
            _ => throw new ArgumentOutOfRangeException()
        };

        private void OnQuestChange(Quest changedQuest)
        {
            if (changedQuest == _quest) _version++;
        }

        private void RebuildVisibleCheckpoints()
        {
            _visibleCheckpoints = new List<Checkpoint>();
            foreach (var checkpointGroup in _quest.CheckpointGroups)
            {
                _visibleCheckpoints.AddRange(checkpointGroup.Checkpoints);
                if (!checkpointGroup.Checkpoints.All(c => c.Reached)) break;
            }
            _version++;
        }

        private void RebuildQuestItems()
        {
            _questItems = _quest.QuestItems.Select(questItem =>
            {
                var amount = _inventory.CountItemsOfType(questItem.ItemType);
                return new QuestItemView(questItem, amount);
            }).Where(questItemView => questItemView.Visibility switch
            {
                Quest.QuestItemVisibility.AlwaysVisible => true,
                Quest.QuestItemVisibility.VisibleAfterCheckpoints => questItemView.Checkpoints.All(c => c.Reached),
                Quest.QuestItemVisibility.VisibleWhenAcquired => questItemView.Amount > 0,
                _ => throw new ArgumentOutOfRangeException()
            }).ToList();
            _version++;
        }

        private void AfterCheckpointsRestored()
        {
            RebuildVisibleCheckpoints();
            RebuildQuestItems();
        }

        private void OnCheckpointUpdated(Checkpoint checkpoint)
        {
            if (_quest.QuestItems.Any(qi => qi.Visibility == Quest.QuestItemVisibility.VisibleAfterCheckpoints
                                            && qi.Checkpoints.Any(c => c == checkpoint))) RebuildQuestItems();
            if (_quest.CheckpointGroups.Any(g => g.Checkpoints.Any(c => c == checkpoint))) RebuildVisibleCheckpoints();
        }

        private void OnInventoryChange()
        {
            RebuildQuestItems();
        }
        #endregion


        #region Lifetime management
        public QuestView(Quest quest, Inventory.Inventory inventory)
        {
            _quest = quest;
            _inventory = inventory;
            Quest.OnChange += OnQuestChange;
            Checkpoint.OnCheckpointUpdated += OnCheckpointUpdated;
            Checkpoint.AfterStateRestored += AfterCheckpointsRestored;
            _inventory.OnChange += OnInventoryChange;
            RebuildVisibleCheckpoints();
            RebuildQuestItems();
        }

        private void ReleaseUnmanagedResources()
        {
            Quest.OnChange -= OnQuestChange;
            Checkpoint.OnCheckpointUpdated -= OnCheckpointUpdated;
            Checkpoint.AfterStateRestored -= AfterCheckpointsRestored;
            _inventory.OnChange -= OnInventoryChange;
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~QuestView() => ReleaseUnmanagedResources();
        #endregion
    }
}
