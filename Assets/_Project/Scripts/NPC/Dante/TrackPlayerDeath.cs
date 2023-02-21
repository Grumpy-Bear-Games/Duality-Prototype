using System;
using DualityGame.Player;
using Games.GrumpyBear.Core.SaveSystem;
using NodeCanvas.Framework;
using UnityEngine;

namespace DualityGame.NPC.Dante
{
    public class TrackPlayerDeath : MonoBehaviour, ISaveableComponent
    {
        [SerializeField] private string _blackboardVariable = "Player Died Since Last Conversation";
        public CauseOfDeath LastCauseOfDeath { get; private set; }
        public int NumberOfPlayerDeaths { get; private set; }

        private void Awake() => CauseOfDeath.OnDeath += OnPlayerDied;
        private void OnDestroy() => CauseOfDeath.OnDeath -= OnPlayerDied;

        private void OnPlayerDied(CauseOfDeath causeOfDeath)
        {
            LastCauseOfDeath = causeOfDeath;
            NumberOfPlayerDeaths++;
            GetComponent<Blackboard>().SetVariableValue(_blackboardVariable, true);
        }

        #region ISaveableComponent
        [Serializable]
        private class SerializableState
        {
            public readonly ObjectGuid CauseOfDeathID;
            public readonly int NumberOfPlayerDeaths;

            public SerializableState(CauseOfDeath causeOfDeath, int numberOfPlayerDeaths)
            {
                CauseOfDeathID = causeOfDeath != null ? causeOfDeath.ObjectGuid : null;
                NumberOfPlayerDeaths = numberOfPlayerDeaths;
            }
        }

        object ISaveableComponent.CaptureState() => new SerializableState(LastCauseOfDeath, NumberOfPlayerDeaths);

        void ISaveableComponent.RestoreState(object state)
        {
            var serializableState = (SerializableState)state;
            LastCauseOfDeath = serializableState.CauseOfDeathID != null ? CauseOfDeath.GetByGuid(serializableState.CauseOfDeathID) : null;
            NumberOfPlayerDeaths = serializableState.NumberOfPlayerDeaths;
        }
        #endregion
    }
}
