using DualityGame.Realm;
using UnityEngine;


namespace DualityGame.Player
{
    public class PlayerLayer : MonoBehaviour
    {
        [SerializeField] private RealmObservable _currentRealm;

        private void OnRealmChange(Realm.Realm newRealm)
        {
            if (newRealm == null) return;
            gameObject.layer = newRealm.PlayerLayer;
        }

        private void OnEnable() => _currentRealm.Subscribe(OnRealmChange);

        private void OnDisable() => _currentRealm.Unsubscribe(OnRealmChange);
    }
}
