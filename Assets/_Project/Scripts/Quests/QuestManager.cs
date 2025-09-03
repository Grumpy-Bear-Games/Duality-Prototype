using Games.GrumpyBear.Core.SaveSystem;
using NodeCanvas.DialogueTrees;
using UnityEngine;

namespace DualityGame.Quests
{
    [RequireComponent(typeof(SaveableEntity))]
    public class QuestManager : MonoBehaviour, ISaveableComponent
    {
        private void OnEnable()
        {
            Quest.OnBeginQuest += OnBeginQuest;
            Quest.OnRevealQuest += OnRevealQuest;
            Quest.OnCompleteQuest += OnCompleteQuest;
        }

        private void OnDisable()
        {
            Quest.OnBeginQuest -= OnBeginQuest;
            Quest.OnRevealQuest -= OnRevealQuest;
            Quest.OnCompleteQuest -= OnCompleteQuest;
        }

        #region Notifications
        private void OnBeginQuest(Quest quest)
        {
            if (quest.Visibility == Quest.QuestVisibility.ShowAutomaticallyWhenOngoing) {
                Notifications.Notifications.Add(quest.NPC.PortraitByMood(Mood.Neutral), $"You started a new quest: {quest.Title}");
            }
        }

        private void OnRevealQuest(Quest quest)
        {
            Notifications.Notifications.Add(quest.NPC.PortraitByMood(Mood.Neutral), $"You discovered a secret quest: {quest.Title}");
        }

        private void OnCompleteQuest(Quest quest)
        {
            Notifications.Notifications.Add(quest.NPC.PortraitByMood(Mood.Happy),$"You completed a quest: {quest.Title}");
        }
        #endregion

        #region ISaveableComponent
        object ISaveableComponent.CaptureState() => Quest.CaptureState();
        void ISaveableComponent.RestoreState(object state) => Quest.RestoreState(state);
        #endregion
    }
}
