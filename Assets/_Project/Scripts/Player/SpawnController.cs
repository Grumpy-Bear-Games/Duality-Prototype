using System.Collections;
using Cinemachine;
using Games.GrumpyBear.Core.LevelManagement;
using Games.GrumpyBear.Core.SaveSystem;
using UnityEngine;

namespace DualityGame.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class SpawnController : MonoBehaviour, ISaveableComponent
    {
        public static SpawnController Instance { get; private set; }
        

        private CharacterController _controller;

        public void MoveToSpawnPoint(string spawnPointID)
        {
            var spawnPoint = SpawnPoint.FindByID(spawnPointID);
            
            Debug.Assert(spawnPoint != null, "spawnPoint != null", this);
            Debug.Assert(spawnPoint.Realm != null, "spawnPoint.Realm != null", this);
            
            if (_controller != null) _controller.enabled = false;
            var positionDelta = spawnPoint.transform.position - transform.position; 
            transform.position = spawnPoint.transform.position;
            CinemachineCore.Instance.OnTargetObjectWarped(transform, positionDelta);
            if (spawnPoint.Realm != null) spawnPoint.Realm.SetActive();
            if (_controller != null) _controller.enabled = true;
        }

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            if (Instance != null)
            {
                Debug.LogError($"SpawnController instance already exists: {Instance.name}", this);
                Destroy(gameObject);
            }

            Instance = this;
        }

        private IEnumerator Start()
        {
            yield return SceneManager.WaitForLoadingCompleted();
            MoveToSpawnPoint(null);
        }

        private void OnDestroy() => Instance = null;
        
        object ISaveableComponent.CaptureState() => transform.position;

        void ISaveableComponent.RestoreState(object state)
        {
            var position = (Vector3)state;
            if (_controller != null) _controller.enabled = false;
            var positionDelta = position - transform.position; 
            transform.position = position;
            CinemachineCore.Instance.OnTargetObjectWarped(transform, positionDelta);
            if (_controller != null) _controller.enabled = true;
        }
    }
}
