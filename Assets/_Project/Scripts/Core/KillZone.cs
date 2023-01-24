using UnityEngine;

namespace DualityGame.Core
{
    public class KillZone : MonoBehaviour
    {
        [field: SerializeField] public string CauseOfDeath { get; private set; }  = "You feel into the abyss!";

        private void OnTriggerEnter(Collider other)
        {
            var killable = other.GetComponent<IKillable>();
            killable?.Kill(CauseOfDeath);
        }
    }
}
