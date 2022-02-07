using DualityGame.Player;
using Games.GrumpyBear.Core.Events;
using UnityEngine;

namespace DualityGame.Core
{
    public class KillZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var killable = other.GetComponent<IKillable>(); 
            if (killable != null) killable.Kill();
        }
    }
}
