using Games.GrumpyBear.Core.SaveSystem;
using UnityEngine;

namespace DualityGame.Quests
{
    [RequireComponent(typeof(SaveableEntity))]
    public class CheckpointManager : MonoBehaviour, ISaveableComponent
    {
        #region ISaveableComponent
        object ISaveableComponent.CaptureState() => Checkpoint.CaptureState();
        void ISaveableComponent.RestoreState(object state) => Checkpoint.RestoreState(state);
        #endregion
    }
}
