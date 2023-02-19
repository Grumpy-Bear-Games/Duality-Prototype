using System.Collections;
using Cinemachine;
using DualityGame.Realm;
using Games.GrumpyBear.LevelManagement;
using UnityEngine;
using UnityEngine.Assertions;

namespace DualityGame.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class SpawnController : MonoBehaviour
    {
        [SerializeField] private RealmObservable _currentRealm;
        [SerializeField] private ObservableSpawnPoint _lastSpawnPoint;
        
        private CharacterController _controller;

        public void Respawn()
        {
            Assert.IsNotNull(_lastSpawnPoint.Value);
            if (_controller != null) _controller.enabled = false;
            var positionDelta = _lastSpawnPoint.Value.transform.position - transform.position; 
            transform.position = _lastSpawnPoint.Value.transform.position;
            CinemachineCore.Instance.OnTargetObjectWarped(transform, positionDelta);
            if (_lastSpawnPoint.Value.Realm != null) _currentRealm.Set(_lastSpawnPoint.Value.Realm);
            if (_controller != null) _controller.enabled = true;
        }
        
        private void Awake() => _controller = GetComponent<CharacterController>();

        private IEnumerator Start()
        {
            yield return SceneManager.WaitForLoadingCompleted();
            _lastSpawnPoint.Set(SpawnPoint.FindInitial());

            if (_lastSpawnPoint.Value == null)
            {
                Debug.LogError("No initial spawn point found");
                yield break;
            }

            Respawn();
        }
    }
}
