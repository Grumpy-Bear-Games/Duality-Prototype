using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Quests.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class QuestLog : MonoBehaviour
    {
        private UIDocument _uiDocument;

        private QuestSlot leftSlot;
        private QuestSlot rightSlot;

        [SerializeField] private List<Quest> _quests; 

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();

            var pages = _uiDocument.rootVisualElement.Query<QuestSlot>().ToList();
            leftSlot = pages[0];
            rightSlot = pages[1];

            leftSlot.Quest = _quests[0];
            rightSlot.Quest = _quests[1];
        }
    }
}
