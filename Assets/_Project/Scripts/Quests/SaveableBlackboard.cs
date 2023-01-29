using DualityGame.SaveSystem;
using NodeCanvas.Framework;
using UnityEngine;

namespace DualityGame.Quests
{
    [RequireComponent(typeof(Blackboard))]
    [RequireComponent(typeof(SaveableEntity))]
    public class SaveableBlackboard : MonoBehaviour, ISaveableComponent
    {
        private Blackboard _blackboard;

        private void Awake() => _blackboard = GetComponent<Blackboard>();

        object ISaveableComponent.CaptureState() => _blackboard.Serialize(null);

        void ISaveableComponent.RestoreState(object state) => _blackboard.Deserialize((string)state, null);
    }
}
