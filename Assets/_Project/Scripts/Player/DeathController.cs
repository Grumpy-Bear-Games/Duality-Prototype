using DualityGame.Core;
using UnityEngine;
using UnityEngine.Events;

namespace DualityGame.Player
{
    public class DeathController : MonoBehaviour, IKillable
    {
        [SerializeField] private UnityEvent _onDie;
        public void Kill() => _onDie.Invoke();
    }
}
