using System;
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
        private bool _hasBeenInitialized;

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
            if (_hasBeenInitialized) yield break;
            _hasBeenInitialized = true;
            MoveToSpawnPoint(null);
        }

        private void OnDestroy() => Instance = null;

        #region ISaveableComponent
        [Serializable]
        private readonly struct SerializedState
        {
            public readonly ObjectGuid RealmID;
            public readonly Vector3 Position;
            public readonly bool HasBeenInitialized;

            public SerializedState(Vector3 position, bool hasBeenInitialized)
            {
                RealmID = Realm.Realm.Current.ObjectGuid;
                Position = position;
                HasBeenInitialized = hasBeenInitialized;
            }
        }

        object ISaveableComponent.CaptureState() => new SerializedState(position: transform.position, hasBeenInitialized: _hasBeenInitialized);

        void ISaveableComponent.RestoreState(object state)
        {
            if (state is not SerializedState serializedState) return;

            _hasBeenInitialized = serializedState.HasBeenInitialized;
            if (_controller != null) _controller.enabled = false;
            var positionDelta = serializedState.Position - transform.position; 
            transform.position = serializedState.Position;
            CinemachineCore.Instance.OnTargetObjectWarped(transform, positionDelta);
            Realm.Realm.GetByGuid(serializedState.RealmID).SetActive();
            if (_controller != null) _controller.enabled = true;
        }
        #endregion
    }
}
