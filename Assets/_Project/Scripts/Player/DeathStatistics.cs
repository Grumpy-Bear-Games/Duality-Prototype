using NodeCanvas.Framework;
using UnityEngine;

namespace DualityGame.Player
{
    [RequireComponent(typeof(Blackboard))]
    public class DeathStatistics : MonoBehaviour
    {
        private const string LastCauseOfDeathBBKey = "Last Cause Of Death";
        private const string NumberOfDeathsBBKey = "Number of Deaths";

        private Blackboard _blackboard;

        private void Awake()
        {
            _blackboard = GetComponent<Blackboard>();
            CauseOfDeath.OnDeath += OnPlayerDied;
        }

        private void OnDestroy() => CauseOfDeath.OnDeath -= OnPlayerDied;

        private void OnPlayerDied(CauseOfDeath causeOfDeath)
        {
            _blackboard.SetVariableValue(LastCauseOfDeathBBKey, causeOfDeath);
            var deaths = _blackboard.GetVariableValue<int>(NumberOfDeathsBBKey);
            _blackboard.SetVariableValue(NumberOfDeathsBBKey, deaths + 1);
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
