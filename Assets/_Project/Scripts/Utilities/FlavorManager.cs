using System.Collections.Generic;
using DualityGame.Utilities.Tasks;
using Games.GrumpyBear.Core.SaveSystem;
using UnityEngine;

namespace DualityGame.Utilities
{
    [RequireComponent(typeof(SaveableEntity))]
    public class FlavorManager : MonoBehaviour, ISaveableComponent
    {
        #region ISaveableComponent
        object ISaveableComponent.CaptureState() => FlavorBranchingNode.CaptureState();

        void ISaveableComponent.RestoreState(object state) => FlavorBranchingNode.RestoreState(state);
        #endregion
    }
}
