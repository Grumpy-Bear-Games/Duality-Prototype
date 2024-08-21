using Games.GrumpyBear.Core.SaveSystem;
using UnityEngine;
using UnityEngine.Events;

namespace DualityGame.Player
{
    [RequireComponent(typeof(SaveableEntity))]
    public class Portal : MonoBehaviour, ISaveableComponent
    {
        [SerializeField] private SpawnSettings _spawnSettings;

        [SerializeField] private SpawnPointReference _spawnPointReference;


        [SerializeField] private UnityEvent _onWarp;
        private bool _active;

        private ParticleSystem _particleSystem;

        public void Activate()
        {
            _active = true;
            _particleSystem.Play();
        }

        private void Awake() => _particleSystem = GetComponentInChildren<ParticleSystem>();

        private void OnTriggerEnter(Collider other)
        {
            if (!_active) return;
            _onWarp?.Invoke();
            _spawnPointReference.SpawnAt(_spawnSettings);
        }

        #region ISaveableComponent
        object ISaveableComponent.CaptureState() => _active;

        public void RestoreState(object state)
        {
            _active = (bool) state;
            if (_active)
            {
                _particleSystem.Simulate(5f);
                _particleSystem.Play();
            }
            else
            {
                _particleSystem.Stop();
            }
        }
        #endregion
    }
}
