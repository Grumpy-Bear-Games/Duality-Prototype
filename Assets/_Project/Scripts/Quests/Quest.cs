using UnityEditor;
using UnityEngine;

namespace DualityGame.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Duality/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] private string _title;
        [SerializeField] private string _description;

        private QuestState _state = QuestState.Inactive;

        public string Title => _title;
        public string Description => _description;
        
        public QuestState State
        {
            get => _state;
            set => _state = value;
        }

        public enum QuestState
        {
            Inactive, Ongoing, Completed, Failed
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode) return;
            _state = QuestState.Inactive;
        }
#endif

    }
}
