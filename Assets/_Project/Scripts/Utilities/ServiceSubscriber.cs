using UnityEngine;

namespace DualityGame.Utilities
{
    public abstract class ServiceSubscriber<T> : MonoBehaviour where T : class
    {
        protected virtual void OnEnable() => ServiceLocator.Subscribe<T>(OnServiceChanged);
        protected virtual void OnDisable() => ServiceLocator.Unsubscribe<T>(OnServiceChanged);
        protected abstract void OnServiceChanged(T service);
    }
}