using UnityEngine;

namespace DualityGame.Realm
{
    public class RealmRenderer : MonoBehaviour
    {
        [SerializeField] private Realm _realm;

        private void OnRealmChange(Realm newRealm)
        {
            if (newRealm == null) return;

            var active = _realm == newRealm;
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = active;
            }
        }
        
        private void OnEnable() => Realm.Subscribe(OnRealmChange);
        private void OnDisable() => Realm.Unsubscribe(OnRealmChange);
    }
}
