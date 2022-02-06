using DualityGame.Player;
using Games.GrumpyBear.Core.Events;
using UnityEngine;

namespace DualityGame.Environment
{
    public class KillZone : MonoBehaviour
    {
        [SerializeField] private VoidEvent _killPlayer;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ThirdPersonFixedController>() == null) return;
            
            _killPlayer.Invoke();
        }
    }
}
