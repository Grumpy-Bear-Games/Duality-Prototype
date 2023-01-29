using DualityGame.SaveSystem;
using UnityEngine;

namespace DualityGame.Quests
{
    [RequireComponent(typeof(SaveableEntity))]
    public class QuestLogController : MonoBehaviour, ISaveableComponent
    {
        [SerializeField] private QuestLog _questLog;

        #region ISaveableComponent
        object ISaveableComponent.CaptureState() => ((ISaveableComponent)_questLog).CaptureState();
        void ISaveableComponent.RestoreState(object state) => ((ISaveableComponent)_questLog).RestoreState(state);
        #endregion
    }
}
