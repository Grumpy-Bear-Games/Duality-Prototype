using DualityGame.Core;
using UnityEngine;
using UnityEngine.Events;

namespace DualityGame.Player
{
    public class DeathController : MonoBehaviour, IKillable
    {
        [SerializeField] private UnityEvent<string> _onDie;
        public void Kill(string causeOfDeath) => _onDie.Invoke(causeOfDeath);
    }
}
