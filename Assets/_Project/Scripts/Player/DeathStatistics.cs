using DualityGame.Core;
using NodeCanvas.Framework;
using UnityEngine;

namespace DualityGame.Player
{
    [RequireComponent(typeof(Blackboard))]
    public class DeathStatistics : MonoBehaviour
    {
        private const string LastCauseOfDeathBBKey = "Last Cause Of Death";
        private const string NumberOfDeathsBBKey = "Number of Deaths";
        private const string DiedSinceLastConversationWithDante = "Died Since Last Conversation With Dante";

        private Blackboard _blackboard;

        private void Awake()
        {
            _blackboard = GetComponent<Blackboard>();
            DeathController.OnPlayerDied += OnPlayerDied;
        }

        private void OnDestroy() => DeathController.OnPlayerDied -= OnPlayerDied;

        private void OnPlayerDied(CauseOfDeath causeOfDeath, WaitForCompletion _)
        {
            _blackboard.SetVariableValue(LastCauseOfDeathBBKey, causeOfDeath);
            var deaths = _blackboard.GetVariableValue<int>(NumberOfDeathsBBKey);
            _blackboard.SetVariableValue(NumberOfDeathsBBKey, deaths + 1);
            _blackboard.SetVariableValue(DiedSinceLastConversationWithDante, true);
        }

        private void Reset()
        {
            var bb = GetComponent<Blackboard>();
            if (bb == null) return;
            bb.AddVariable<CauseOfDeath>(LastCauseOfDeathBBKey);
            bb.SetVariableValue(NumberOfDeathsBBKey, 0);
        }
    }
}
