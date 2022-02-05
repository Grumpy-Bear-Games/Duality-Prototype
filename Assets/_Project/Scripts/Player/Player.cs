using DualityGame.Realm;
using UnityEngine;

namespace DualityGame
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Realm.Realm _spawnRealm;
        [SerializeField] private RealmManager _realmManager;

        public void Spawn()
        {
            transform.position = _spawnPoint.position;
            if (_realmManager.CurrentRealm.Value != _spawnRealm) _realmManager.Warp();
        }
    }
}
