using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Quests.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class QuestLogUI : MonoBehaviour
    {
        private UIDocument _uiDocument;

        private VisualElement _frame;
        private ListView _questList;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _frame = _uiDocument.rootVisualElement.Q<VisualElement>("QuestLogBook");
            _questList = _uiDocument.rootVisualElement.Q<ListView>("QuestIndex");

            _questList.makeNoneElement += () =>
            {
                var label = new Label
                {
                    text = "I haven't started any quests yet...",
                };
                label.AddToClassList("unity-list-view__empty-label");
                return label;
            };
        }

        private void OnEnable()
        {
            Show();
            FocusFirstOngoingQuest();
        }

        private void FocusFirstOngoingQuest()
        {
            _questList.Focus();

            if (_questList.itemsSource is not List<QuestView> quests) return;
            if (quests.Count < 1) return;

            var index = quests.Count - 1;
            var firstOngoingQuest = quests.FirstOrDefault(q => !q.HasCompleted);
            if (firstOngoingQuest != null)
            {
                index = quests.IndexOf(firstOngoingQuest);
            }
            _questList.SetSelection(index);
            _questList.ScrollToItem(index);
        }

        public void OnDisable() => Hide();

        private void Hide() => _frame?.RemoveFromClassList("Shown");

        private void Show() => _frame?.AddToClassList("Shown");
    }
}
