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

        [SerializeField] private int _questIndex;

        private readonly List<Quest.QuestState> _questEntries = new();

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _frame = _uiDocument.rootVisualElement.Q<VisualElement>("QuestLogBook");

            _queryLogPage = _uiDocument.rootVisualElement.Q<QuestLogPage>();

            _questList = _uiDocument.rootVisualElement.Q<ListView>();
            _questList.makeItem = () => new QuestIndexItem();
            _questList.bindItem = (item, idx) => (item as QuestIndexItem).QuestState = _questEntries[idx];
            _questList.itemsSource = _questEntries;
            _questList.selectionChanged += _ => _queryLogPage.QuestState = _questList.selectedItem as Quest.QuestState;
        }

        private void UpdateQuests()
        {
            var previousSelected = _questList.selectedItem as Quest.QuestState;
            
            _questEntries.Clear();
            _questEntries.AddRange(Quest.VisibleQuests);
            _questEntries.Sort((a,b) => (int)(a.Started - b.Started));
            _questList.RefreshItems();

            if (_questEntries.Count == 0) _queryLogPage.QuestState = null;
            
            if (previousSelected == null) {
                _questList.SetSelection(0);
            } else if (_questEntries.Contains(previousSelected)) {
                _questList.SetSelection(_questEntries.IndexOf(previousSelected));
            } else {
                _questList.SetSelection(0);
            }
        }

        private void OnQuestyChange(Quest.QuestState _) => UpdateQuests();

        private void OnEnable()
        {
            Quest.OnChange += OnQuestyChange;
            Quest.AfterStateRestored += UpdateQuests;
            UpdateQuests();
            Show();
        }


        public void OnDisable()
        {
            Hide();
            Quest.OnChange -= OnQuestyChange;
            Quest.AfterStateRestored -= UpdateQuests;
        }

        private void Hide() => _frame?.RemoveFromClassList("Shown");

        private void Show() => _frame?.AddToClassList("Shown");
    }
}
