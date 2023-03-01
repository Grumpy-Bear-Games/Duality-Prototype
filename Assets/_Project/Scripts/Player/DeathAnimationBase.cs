using UnityEngine;
using UnityEngine.Events;

namespace DualityGame.Player
{
    public abstract class DeathAnimationBase: MonoBehaviour
    {
        [SerializeField] private UnityEvent _onTrigger;
        [SerializeField] private UnityEvent _onReset;

        public virtual void Trigger() => _onTrigger.Invoke();

        public virtual void ResetPlayer() => _onReset.Invoke();
    }
}
