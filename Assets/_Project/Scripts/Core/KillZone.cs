using DualityGame.Player;
using UnityEngine;

namespace DualityGame.Core
{
    public class KillZone : MonoBehaviour
    {
        [SerializeField] private CauseOfDeath _causeOfDeath;

        private void OnTriggerEnter(Collider other)
        {
            var killable = other.GetComponent<IKillable>();
            killable?.Kill(_causeOfDeath);
        }
    }
}
