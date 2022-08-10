using UnityEngine;

namespace DualityGame.Realm
{
    public class RealmRenderer : MonoBehaviour
    {
        [SerializeField] private Realm _realm;
        [SerializeField] private RealmObservable _currentRealm;

        private void OnRealmChange(Realm newRealm)
        {
            if (newRealm == null) return;

            var active = _realm == newRealm;
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = active;
            }
        }
        
        private void OnEnable() => _currentRealm.Subscribe(OnRealmChange);
        private void OnDisable() => _currentRealm.Unsubscribe(OnRealmChange);
    }
}
