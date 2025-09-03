using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Quests
{
    [CreateAssetMenu(fileName = "Quests View", menuName = "Duality/Quests View", order = 0), GeneratePropertyBag]
    public class QuestsView : ScriptableObject, IDataSourceViewHashProvider
    {
        private long _version;
        private readonly Dictionary<Quest, QuestView> _questViewCache = new();
        private List<QuestView> _visibleQuests = new();

        [SerializeField] private Inventory.Inventory _inventory;

        long IDataSourceViewHashProvider.GetViewHashCode() => _version;

        #region Properties
        [CreateProperty]
        public IReadOnlyList<QuestView> VisibleQuests => _visibleQuests;

        #endregion

        #region Lifetime management
        private void OnEnable()
        {
            Quest.OnChange -= OnQuestChange;
            Quest.OnChange += OnQuestChange;

            Checkpoint.OnCheckpointUpdated -= OnCheckpointChange;
            Checkpoint.OnCheckpointUpdated += OnCheckpointChange;

            if (_inventory != null)
            {
                _inventory.OnChange -= OnInventoryChange;
                _inventory.OnChange += OnInventoryChange;
            }
            RebuildView();
        }

        private void OnDisable()
        {
            Quest.OnChange -= OnQuestChange;
            Checkpoint.OnCheckpointUpdated -= OnCheckpointChange;
            if (_inventory != null) _inventory.OnChange -= OnInventoryChange;
        }
        #endregion

        #region Private methods
        private void RebuildView()
        {
            _visibleQuests = Quest.AllQuests
                .Where(quest => quest.IsVisible)
                .Select(quest =>
                {
                    if (_questViewCache.TryGetValue(quest, out var questView)) return questView;
                    questView = new QuestView(quest, _inventory);
                    _questViewCache[quest] = questView;
                    return questView;
                })
                .ToList();
            _visibleQuests.Sort();
            _version++;
        }

        private void OnQuestChange(Quest _) => RebuildView();

        private void OnCheckpointChange(Checkpoint _) => _version++;

        private void OnInventoryChange() => _version++;

        #endregion
    }
}
