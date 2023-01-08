using DualityGame.VFX;
using UnityEngine;

namespace DualityGame.Core
{
    public class KillZone : MonoBehaviour
    {
        [SerializeField] private string _causeOfDeath = "You feel into the abyss!";
        
        private void OnTriggerEnter(Collider other)
        {
            var killable = other.GetComponent<IKillable>();
            if (killable == null) return;
            
            DeathScreen.CauseOfDeath.Set(_causeOfDeath);
            killable.Kill();
        }
    }
}
