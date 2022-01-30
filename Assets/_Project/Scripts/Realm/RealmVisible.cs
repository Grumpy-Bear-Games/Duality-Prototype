using UnityEngine;
using UnityEngine.Rendering;

namespace DualityGame.Realm
{
    public class RealmVisible : MonoBehaviour
    {
        [SerializeField] private RealmManager _realmManager;
        
        private Renderer[] _renderers;
        private Volume[] _volumes;
        private void Awake()
        {
            _renderers = GetComponentsInChildren<Renderer>();
            _volumes = GetComponentsInChildren<Volume>();
        }

        private void OnEnable() => _realmManager.CurrentRealm.Subscribe(OnChangeRealm);

        private void OnDisable() => _realmManager.CurrentRealm.Unsubscribe(OnChangeRealm);

        private void OnChangeRealm(Realm realm)
        {
            var visible = realm.LevelLayer == gameObject.layer;
            foreach (var renderer in _renderers) renderer.enabled = visible;
            foreach (var volume in _volumes) volume.enabled = visible;
        }
    }
}
