using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DualityGame.Realm
{
    public class RealmVisible : MonoBehaviour
    {
        [SerializeField] private Realm _realm;
        
        private Renderer[] _renderers;
        private Volume[] _volumes;
        private DecalProjector[] _decalProjectors;
        private void Awake()
        {
            _volumes = GetComponentsInChildren<Volume>(true);
        }

        private void OnEnable() => Realm.Subscribe(OnChangeRealm);

        private void OnDisable() => Realm.Unsubscribe(OnChangeRealm);

        private void OnChangeRealm(Realm realm)
        {
            if (realm == null) return;
            var visible = realm == _realm;
            foreach (var volume in _volumes) volume.enabled = visible;
        }
        
        #if UNITY_EDITOR
        private void Reset() => _realm = Realm.FromLayer(gameObject.layer);

        public static class Fields
        {
            public static readonly string Realm = nameof(_realm);
        }
        #endif
    }
}
