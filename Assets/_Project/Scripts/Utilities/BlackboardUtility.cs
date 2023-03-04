using NodeCanvas.Framework;
using UnityEngine;

namespace DualityGame.Utilities
{
    public class BlackboardUtility : MonoBehaviour
    {
        private Blackboard _blackboard;
        private void Awake() => _blackboard = GetComponent<Blackboard>();

        public void SetBooleanTrue(string key) => _blackboard.SetVariableValue(key, true);
        public void SetBooleanFalse(string key) => _blackboard.SetVariableValue(key, false);
        public void ToggleBoolean(string key)
        {
            var value = _blackboard.GetVariableValue<bool>(key);
            _blackboard.SetVariableValue(key, !value);
        }

        public void IncrementInteger(string key)
        {
            var value = _blackboard.GetVariableValue<int>(key);
            _blackboard.SetVariableValue(key, value + 1);
        }
        public void DecrementInteger(string key)
        {
            var value = _blackboard.GetVariableValue<int>(key);
            _blackboard.SetVariableValue(key, value - 1);
        }
    }
}
