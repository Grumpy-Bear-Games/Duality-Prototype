using Games.GrumpyBear.Core.SaveSystem;
using UnityEngine;

namespace DualityGame.Quests
{
    [RequireComponent(typeof(SaveableEntity))]
    public class QuestManager : MonoBehaviour, ISaveableComponent
    {
        #region ISaveableComponent
        object ISaveableComponent.CaptureState() => Quest.CaptureState();
        void ISaveableComponent.RestoreState(object state) => Quest.RestoreState(state);
        #endregion
    }
}
