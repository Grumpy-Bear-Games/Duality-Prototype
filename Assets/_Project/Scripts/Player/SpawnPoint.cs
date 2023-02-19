using System.Linq;
using UnityEngine;

namespace DualityGame.Player
{
    public class SpawnPoint : MonoBehaviour
    {
        public const string InitialTag = "Respawn";
        
        [field: SerializeField] public string ID;
        [field: SerializeField] public Realm.Realm Realm { get; private set; }

        public static SpawnPoint FindInitial() => FindObjectsOfType<SpawnPoint>()
            .FirstOrDefault(spawnPoint => spawnPoint.CompareTag(InitialTag));

        public static SpawnPoint FindByID(string id) => FindObjectsOfType<SpawnPoint>()
            .FirstOrDefault(spawnPoint => spawnPoint.ID == id);
    }
}
