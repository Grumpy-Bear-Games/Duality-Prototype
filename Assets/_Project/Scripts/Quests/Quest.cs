using Games.GrumpyBear.Core.SaveSystem;
using NodeCanvas.DialogueTrees;
using UnityEngine;

namespace DualityGame.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Duality/Quest", order = 0)]
    public class Quest : SerializableScriptableObject<Quest>
    {
        #region Serialized fields
        [field: SerializeField] public string Title { get; private set; }
        [field: Multiline][field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public ActorAsset NPC { get; private set; }
        [field: SerializeField] public Realm.Realm Realm { get; private set; }
        [field: SerializeField] public QuestVisibility Visibility { get; private set; } =
            QuestVisibility.ShowAutomaticallyWhenOngoing;
        #endregion

        public string TitleWithNPC => NPC != null ? $"{Title} ({NPC.Name})" : Title;
        
        #region Public enums
        public enum QuestVisibility
        {
            ShowAutomaticallyWhenOngoing,
            ManualVisibility
        }
        #endregion
    }
}
