using UnityEngine;
using UnityEngine.Events;

namespace DualityGame.Core
{
    public class KillZone : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onKill;
        
        private void OnTriggerEnter(Collider other)
        {
            var killable = other.GetComponent<IKillable>();
            if (killable != null)
            {
                _onKill.Invoke();
                killable.Kill();
            }
        }
    }
}
