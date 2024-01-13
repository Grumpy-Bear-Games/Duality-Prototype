using System.Collections.Generic;
using UnityEngine;

namespace DualityGame.Player
{
    public class SpawnPoint : MonoBehaviour
    {
        private const string InitialTag = "Respawn";

        private static SpawnPoint _initialSpawnPoint;
        private static readonly Dictionary<string, SpawnPoint> _spawnPoints = new();

        public static SpawnPoint FindByID(string id)
        {
            if (string.IsNullOrEmpty(id)) return _initialSpawnPoint;
            _spawnPoints.TryGetValue(id, out var spawnPoint);
            return spawnPoint;
        }


        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public Realm.Realm Realm { get; private set; }

        private void Awake()
        {
            if (!string.IsNullOrEmpty(ID)) _spawnPoints[ID] = this;
            if (CompareTag(InitialTag)) _initialSpawnPoint = this;
        }

        private void OnDestroy()
        {
            if (_initialSpawnPoint == this) _initialSpawnPoint = null;
            if (!string.IsNullOrEmpty(ID) && _spawnPoints.TryGetValue(ID, out var spawnPoint) && spawnPoint == this)
                _spawnPoints.Remove(ID);
        }
    }
}
