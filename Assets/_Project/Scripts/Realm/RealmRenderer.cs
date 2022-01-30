using UnityEngine;

namespace DualityGame.Realm
{
    public class RealmRenderer : MonoBehaviour
    {
        [SerializeField] private Realm _realm;
        [SerializeField] private RealmManager _realmManager;


        private void OnRealmChange(Realm newRealm)
        {
            if (newRealm == null) return;

            var active = _realm == newRealm;
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = active;
            }
        }
        
        private void OnEnable() => _realmManager.CurrentRealm.Subscribe(OnRealmChange);
        private void OnDisable() => _realmManager.CurrentRealm.Unsubscribe(OnRealmChange);
    }
}
