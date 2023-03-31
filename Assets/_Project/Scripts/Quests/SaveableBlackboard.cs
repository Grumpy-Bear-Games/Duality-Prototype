using System;
using System.Collections.Generic;
using System.Linq;
using Games.GrumpyBear.Core.SaveSystem;
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

        #region ISaveableComponent

        [Serializable]
        private class SerializableState
        {
            public readonly string BlackboardState;
            public readonly List<ObjectGuid> ObjectGuids;

            public List<UnityEngine.Object> AsObjectReferences => ObjectGuids.Select(objectGuid =>
            {
                if (objectGuid == null) return null;
                return SerializableScriptableObject.GetByGuid(objectGuid) as UnityEngine.Object;
            }).ToList();

            public SerializableState(string blackboardState, List<UnityEngine.Object> references)
            {
                BlackboardState = blackboardState;
                ObjectGuids = references.Select(obj =>
                {
                    if (obj == null || obj is not SerializableScriptableObject sso) return null;
                    return sso.ObjectGuid;
                }).ToList();
            }
        }

        #endregion
        object ISaveableComponent.CaptureState()
        {
            var references = new List<UnityEngine.Object>();
            var serialized = _blackboard.Serialize(references);
            return new SerializableState(serialized, references);
        }

        void ISaveableComponent.RestoreState(object state)
        {
            var serializableState = (SerializableState)state;
            _blackboard.Deserialize(serializableState.BlackboardState, serializableState.AsObjectReferences);
        }
    }
}
