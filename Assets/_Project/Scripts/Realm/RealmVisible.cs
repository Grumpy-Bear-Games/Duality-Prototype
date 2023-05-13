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
            _renderers = GetComponentsInChildren<Renderer>();
            _volumes = GetComponentsInChildren<Volume>();
            _decalProjectors = GetComponentsInChildren<DecalProjector>();
        }

        private void OnEnable() => Realm.Subscribe(OnChangeRealm);

        private void OnDisable() => Realm.Unsubscribe(OnChangeRealm);

        private void OnChangeRealm(Realm realm)
        {
            if (realm == null) return;
            var visible = realm == _realm;
            foreach (var renderer in _renderers) renderer.enabled = visible;
            foreach (var volume in _volumes) volume.enabled = visible;
            foreach (var decalProjector in _decalProjectors) decalProjector.enabled = visible;
        }
        
        #if UNITY_EDITOR
        private void Reset() => _realm = Realm.FromLayer(gameObject.layer);

        public static class Fields
        {
            public static string Realm = nameof(_realm);
        }
        #endif
    }
}
