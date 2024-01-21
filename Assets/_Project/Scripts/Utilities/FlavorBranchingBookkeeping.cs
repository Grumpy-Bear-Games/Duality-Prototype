using System.Collections.Generic;
using Games.GrumpyBear.Core.SaveSystem;
using UnityEngine;

namespace DualityGame.Utilities
{
    [RequireComponent(typeof(SaveableEntity))]
    public class FlavorBranchingBookkeeping : MonoBehaviour, ISaveableComponent
    {
        private Dictionary<string, int> _flavorBranchingCounters = new();

        public int Step(string counterName)
        {
            _flavorBranchingCounters.TryAdd(counterName, 0);
            return _flavorBranchingCounters[counterName]++;
        }

        #region ISaveableComponent
        object ISaveableComponent.CaptureState() => _flavorBranchingCounters;

        void ISaveableComponent.RestoreState(object state)
        {
            if (state is not Dictionary<string, int> flavorBranchingCounters)
            {
                Debug.LogError("State is not a Dictionary<string, int>", this);
                Debug.Break();
                return;
            }
            _flavorBranchingCounters = flavorBranchingCounters;
        }
        #endregion
    }
}
