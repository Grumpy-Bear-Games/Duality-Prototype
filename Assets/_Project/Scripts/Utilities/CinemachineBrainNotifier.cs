using Cinemachine;
using DualityGame.VFX;
using UnityEngine;

namespace DualityGame.Utilities
{
    [RequireComponent(typeof(CinemachineBrain))]
    public class CinemachineBrainNotifier : MonoBehaviour
    {
        [SerializeField] private CinemachineBrainObservable _cinemachineBrainObservable;
        [SerializeField] private bool _registerOnAwake;

        private void Awake()
        {
            if (!_registerOnAwake) return;
            var brain = GetComponent<CinemachineBrain>();
            _cinemachineBrainObservable.Set(brain);
        }

        private void OnEnable()
        {
            if (_registerOnAwake) return;
            var brain = GetComponent<CinemachineBrain>();
            _cinemachineBrainObservable.Set(brain);
        }

        private void OnDisable()
        {
            if (_registerOnAwake) return;
            var brain = GetComponent<CinemachineBrain>();
            if (_cinemachineBrainObservable.Value != brain) return;
            _cinemachineBrainObservable.Set(null);
        }

        private void OnDestroy()
        {
            if (!_registerOnAwake) return;
            var brain = GetComponent<CinemachineBrain>();
            if (_cinemachineBrainObservable.Value != brain) return;
            _cinemachineBrainObservable.Set(null);
        }
    }
}
