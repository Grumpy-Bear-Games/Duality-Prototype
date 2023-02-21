using UnityEngine;


namespace DualityGame.Player
{
    public class PlayerLayer : MonoBehaviour
    {
        private void OnRealmChange(Realm.Realm newRealm)
        {
            if (newRealm == null) return;
            gameObject.layer = newRealm.PlayerLayer;
        }

        private void OnEnable() => Realm.Realm.Subscribe(OnRealmChange);

        private void OnDisable() => Realm.Realm.Unsubscribe(OnRealmChange);
    }
}
