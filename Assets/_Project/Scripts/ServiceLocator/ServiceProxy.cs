using UnityEngine;

namespace DualityGame.ServiceLocator
{
    public abstract class ServiceProxy<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private T _service;
        [SerializeField] private ServiceRegistrationLifetime _lifetime = ServiceRegistrationLifetime.WhenEnabled;

        protected void Awake()
        {
            if (_lifetime == ServiceRegistrationLifetime.WholeLifetime) ServiceLocator.Register(_service);
        }

        protected void OnDestroy()
        {
            if (_lifetime == ServiceRegistrationLifetime.WholeLifetime) ServiceLocator.Unregister(_service);
        }

        protected void OnEnable()
        {
            if (_lifetime == ServiceRegistrationLifetime.WhenEnabled) ServiceLocator.Register(_service);
        }

        protected void OnDisable()
        {
            if (_lifetime == ServiceRegistrationLifetime.WhenEnabled) ServiceLocator.Unregister(_service);
        }

        private void Reset() => _service = GetComponentInChildren<T>();
    }
}
