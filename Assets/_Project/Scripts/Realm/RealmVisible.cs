using UnityEngine;
using UnityEngine.Rendering;

namespace DualityGame.Realm
{
    public class RealmVisible : MonoBehaviour
    {
        [SerializeField] private RealmObservable _currentRealm;
        
        private Renderer[] _renderers;
        private Volume[] _volumes;
        private void Awake()
        {
            _renderers = GetComponentsInChildren<Renderer>();
            _volumes = GetComponentsInChildren<Volume>();
        }

        private void OnEnable() => _currentRealm.Subscribe(OnChangeRealm);

        private void OnDisable() => _currentRealm.Unsubscribe(OnChangeRealm);

        private void OnChangeRealm(Realm realm)
        {
            if (realm == null) return;
            var visible = realm.LevelLayer == gameObject.layer;
            foreach (var renderer in _renderers) renderer.enabled = visible;
            foreach (var volume in _volumes) volume.enabled = visible;
        }
    }
}
