using System.Collections;
using DualityGame.Realm;
using Games.GrumpyBear.Core.Observables.ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace DualityGame.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class SpawnController : MonoBehaviour
    {
        [SerializeField] private RealmObservable _currentRealm;
        [SerializeField] private GameObjectObservable _spawnPoint;
        [SerializeField] private Realm.Realm _startingRealm;
        
        private CharacterController _controller;

        public void Respawn()
        {
            Assert.IsNotNull(_spawnPoint.Value);
            if (_controller != null) _controller.enabled = false;
            transform.position = _spawnPoint.Value.transform.position;
            _currentRealm.Set(_startingRealm);
            if (_controller != null) _controller.enabled = true;
        }

        private void Awake() => _controller = GetComponent<CharacterController>();

        private IEnumerator Start()
        {
            while (_spawnPoint.Value == null) yield return null;
            Respawn();
        }
    }
}
