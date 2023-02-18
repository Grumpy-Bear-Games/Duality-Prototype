using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Quests.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class QuestLogUI : MonoBehaviour
    {
        private UIDocument _uiDocument;

        private VisualElement _frame;
        private QuestLogPage _queryLogPage;
        private ListView _questList;

        private int NumberOfQuests => _questEntries.Count;
        [SerializeField] private int _questIndex;

        private readonly List<QuestLog.QuestEntry> _questEntries = new();
        [SerializeField] private QuestLog _questLog;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _frame = _uiDocument.rootVisualElement.Q<VisualElement>("QuestLogBook");

            _queryLogPage = _uiDocument.rootVisualElement.Q<QuestLogPage>();

            _questList = _uiDocument.rootVisualElement.Q<ListView>();
            _questList.makeItem = () => new QuestIndexItem();
            _questList.bindItem = (item, idx) => (item as QuestIndexItem).QuestEntry = _questEntries[idx];
            _questList.itemsSource = _questEntries;
            _questList.selectionChanged += _ => _queryLogPage.QuestEntry = _questList.selectedItem as QuestLog.QuestEntry;
        }

        private void OnQueryLogChange()
        {
            var previousSelected = _questList.selectedItem as QuestLog.QuestEntry;
            
            _questEntries.Clear();
            _questEntries.AddRange(_questLog.Entries);
            _questEntries.Sort((a,b) => (int)(a.Started - b.Started));
            _questList.RefreshItems();

            if (_questEntries.Count == 0) _queryLogPage.QuestEntry = null;
            
            if (previousSelected == null) {
                _questList.SetSelection(0);
            } else if (_questEntries.Contains(previousSelected)) {
                _questList.SetSelection(_questEntries.IndexOf(previousSelected));
            } else {
                _questList.SetSelection(0);
            }
        }

        private void OnEnable()
        {
            _questLog.OnChange += OnQueryLogChange;
            OnQueryLogChange();
            Show();
        }

        public void OnDisable()
        {
            Hide();
            _questLog.OnChange -= OnQueryLogChange;
        }

        private void Hide() => _frame?.AddToClassList("Hidden");

        private void Show() => _frame?.RemoveFromClassList("Hidden");
    }
}
