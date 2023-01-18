using System;
using System.Collections.Generic;
using NodeCanvas.DialogueTrees;
using UnityEditor;
using UnityEngine;

namespace DualityGame.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Duality/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        #region Serialized fields
        [field: SerializeField] public string Title { get; private set; }
        [field: Multiline][field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public ActorAsset NPC { get; private set; }
        [field: SerializeField] public Realm.Realm Realm { get; private set; }
        [field: SerializeField] public QuestVisibility Visibility { get; private set; } =
            QuestVisibility.ShowAutomaticallyWhenOngoing;
        #endregion
        
        #region Public properties
        public QuestState State
        {
            get => _state;
            set
            {
                if (_state == value) return;
                _state = value;
                if (_state == QuestState.Ongoing) QuestStartedTimestamp = DateTime.Now.Ticks;
                if (Visibility == QuestVisibility.ManualVisibility) return;
                Hidden = (_state == QuestState.Inactive);
            }
        }

        public bool Hidden { get; private set; } = true;

        public long QuestStartedTimestamp { get; private set; } = 0;
        #endregion

        
        #region Private fields
        private QuestState _state = QuestState.Inactive;
        #endregion


        #region Public methods
        public void Reveal() => Hidden = false;
        #endregion


        #region Public enums
        public enum QuestState
        {
            Inactive,
            Ongoing,
            Completed,
            Failed,
        }

        public enum QuestVisibility
        {
            ShowAutomaticallyWhenOngoing,
            ManualVisibility
        }
        #endregion


        public static readonly HashSet<Quest> AllQuests = new();

        #region Life-cycle management
        private void OnEnable()
        {
            AllQuests.Add(this);
#if UNITY_EDITOR
            if (!EditorApplication.isPlayingOrWillChangePlaymode) return;
            State = QuestState.Inactive;
            Hidden = true;
            QuestStartedTimestamp = 0;
#endif
        }
        #endregion
    }
}
