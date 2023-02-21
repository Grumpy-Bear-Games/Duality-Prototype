using System.Collections.Generic;
using UnityEngine;

namespace DualityGame.Player
{
    public class SpawnPoint : MonoBehaviour
    {
        public const string InitialTag = "Respawn";

        public static SpawnPoint InitialSpawnPoint { get; private set; }
        private static readonly Dictionary<string, SpawnPoint> _spawnPoints = new();

        public static SpawnPoint FindByID(string id)
        {
            if (string.IsNullOrEmpty(id)) return InitialSpawnPoint;
            _spawnPoints.TryGetValue(id, out var spawnPoint);
            return spawnPoint;
        }


        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public Realm.Realm Realm { get; private set; }

        private void Awake()
        {
            if (!string.IsNullOrEmpty(ID)) _spawnPoints[ID] = this;
            if (CompareTag(InitialTag)) InitialSpawnPoint = this;
        }

        private void OnDestroy()
        {
            if (InitialSpawnPoint == this) InitialSpawnPoint = null;
            if (!string.IsNullOrEmpty(ID) && _spawnPoints.TryGetValue(ID, out var spawnPoint) && spawnPoint == this)
                _spawnPoints.Remove(ID);
        }
    }
}
