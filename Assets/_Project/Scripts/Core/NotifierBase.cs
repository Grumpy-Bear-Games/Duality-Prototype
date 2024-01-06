using System.Collections.Generic;
using Games.GrumpyBear.Core.Observables.ScriptableObjects;
using UnityEngine;

namespace DualityGame.Core
{
    public class NotifierBase<T, TObservable> : MonoBehaviour where TObservable : Observable<T>
    {
        [SerializeField] private TObservable _observable;
        [SerializeField] private bool _registerOnAwake;
        private T _value;

        private void Awake()
        {
            _value = GetComponent<T>();
            if (!_registerOnAwake) return;
            _observable.Set(_value);
        }

        private void OnEnable()
        {
            if (_registerOnAwake) return;
            _observable.Set(_value);
        }

        private void OnDisable()
        {
            if (_registerOnAwake) return;
            if (!EqualityComparer<T>.Default.Equals(_observable.Value, _value)) return;
            _observable.Set(default);
        }

        private void OnDestroy()
        {
            if (!_registerOnAwake) return;
            if (!EqualityComparer<T>.Default.Equals(_observable.Value, _value)) return;
            _observable.Set(default);
        }
    }
}
