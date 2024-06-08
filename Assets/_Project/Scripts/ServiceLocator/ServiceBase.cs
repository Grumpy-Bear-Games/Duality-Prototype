using DualityGame.Utilities;
using UnityEngine;

namespace DualityGame.ServiceLocator
{
    public abstract class ServiceBase<T> : MonoBehaviour where T : ServiceBase<T>
    {
        [SerializeField] private ServiceRegistrationLifetime _lifetime = ServiceRegistrationLifetime.WhenEnabled;

        protected virtual void Awake()
        {
            if (_lifetime == ServiceRegistrationLifetime.WholeLifetime) ServiceLocator.Register(this as T);
        }

        protected virtual void OnDestroy()
        {
            if (_lifetime == ServiceRegistrationLifetime.WholeLifetime) ServiceLocator.Unregister(this as T);
        }

        protected virtual void OnEnable()
        {
            if (_lifetime == ServiceRegistrationLifetime.WhenEnabled) ServiceLocator.Register(this as T);
        }

        protected virtual void OnDisable()
        {
            if (_lifetime == ServiceRegistrationLifetime.WhenEnabled) ServiceLocator.Unregister(this as T);
        }
    }

    public class Player : ServiceBase<Player> { }
}
