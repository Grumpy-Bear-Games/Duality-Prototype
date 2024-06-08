using System;
using System.Collections.Generic;

namespace DualityGame.ServiceLocator
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();
        private static readonly Dictionary<Type, List<Delegate>> _onRegister = new();

        public static void Register<T>(T service, bool allowReplace = false) where T : class
        {
            if (service is null) throw new ArgumentNullException(nameof(service));
            var serviceType = typeof(T);

            if (!allowReplace && _services.TryGetValue(serviceType, out var existingService))
            {
                if (!ReferenceEquals(existingService, service))
                {
                    throw new ArgumentException($"Service of type {serviceType.Name} is already registered: {existingService}");
                }

            }
            _services[serviceType] = service;
            Notify(serviceType, service);
        }

        public static void RegisterAs(Type serviceType, object service, bool allowReplace = false)
        {
            if (!serviceType.IsInstanceOfType(service))
            {
                throw new ArgumentException($"Type of {service} ({service.GetType().Name}) is not {serviceType.Name}");
            }

            if (!allowReplace && _services.TryGetValue(serviceType, out var existingService))
            {
                if (!ReferenceEquals(existingService, service))
                {
                    throw new ArgumentException($"Service of type {serviceType.Name} is already registered: {existingService}");
                }

            }
            _services[serviceType] = service;
            Notify(serviceType, service);
        }

        public static void Unregister<T>(T service) where T : class
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            var serviceType = typeof(T);

            if (!_services.TryGetValue(serviceType, out var existingService)) return;
            if (ReferenceEquals(existingService, service)) _services.Remove(serviceType);
            Notify<T>(serviceType, null);
        }

        public static bool TryGet<T>(out T service) where T : class
        {
            if (_services.TryGetValue(typeof(T), out var obj))
            {
                service = obj as T;
                return true;
            }

            service = null;
            return false;
        }

        public static T Get<T>() where T : class
        {
            var serviceType = typeof(T);
            if (_services.TryGetValue(serviceType, out var service)) return service as T;
            throw new ArgumentException($"No service of type {serviceType.Name} is registered");
        }

        public static void Subscribe<T>(Action<T> callback) where T : class
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            if (!_onRegister.TryGetValue(typeof(T), out var callbacks))
            {
                callbacks = new List<Delegate>();
                _onRegister[typeof(T)] = callbacks;
            }
            callbacks.Add(callback);

            TryGet(out T currentService);
            callback.Invoke(currentService);
        }

        public static void Unsubscribe<T>(Action<T> callback) where T : class
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            if (!_onRegister.TryGetValue(typeof(T), out var callbacks)) return;
            callbacks.Remove(callback);
        }

        private static void Notify<T>(Type serviceType, T service) where T : class
        {
            if (!_onRegister.TryGetValue(serviceType, out var callbacks)) return;
            foreach (var callback in callbacks)
            {
                if (callback is Action<T> typedCallback) typedCallback.Invoke(service);
            }
        }
    }
}
