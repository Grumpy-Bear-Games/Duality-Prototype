using DualityGame.Realm;
using UnityEngine;

namespace DualityGame.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Realm.Realm _spawnRealm;

        public void Spawn()
        {
            transform.position = _spawnPoint.position;
        }
    }
}
