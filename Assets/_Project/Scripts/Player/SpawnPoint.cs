using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DualityGame.Player
{
    public class SpawnPoint : MonoBehaviour
    {
        private const string InitialTag = "Respawn";

        private static SpawnPoint _initialSpawnPoint;
        private static readonly Dictionary<SpawnPointReference, SpawnPoint> _spawnPoints = new();

        public static SpawnPoint GetByReference(SpawnPointReference id)
        {
            if (id == null) return _initialSpawnPoint;
            _spawnPoints.TryGetValue(id, out var spawnPoint);
            return spawnPoint;
        }

        [field: SerializeField] public SpawnPointReference SpawnPointReference { get; private set; }
        public Realm.Realm Realm => SpawnPointReference != null ? SpawnPointReference.Realm : null;

        private void Awake()
        {
            if (SpawnPointReference != null) _spawnPoints[SpawnPointReference] = this;
            if (CompareTag(InitialTag)) _initialSpawnPoint = this;
        }

        private void OnDestroy()
        {
            if (_initialSpawnPoint == this) _initialSpawnPoint = null;
            if (SpawnPointReference != null && _spawnPoints.TryGetValue(SpawnPointReference, out var spawnPoint) && spawnPoint == this)
                _spawnPoints.Remove(SpawnPointReference);
        }
    }
}
