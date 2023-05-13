using UnityEngine;
using UnityEngine.Rendering;

namespace DualityGame.VFX.Skybox
{
    public class SkyboxManager : MonoBehaviour
    {
        [SerializeField] private Realm.Realm _realm;
        [SerializeField] private SkyboxSettings _skyboxSettings;
        
        private void OnEnable() => Realm.Realm.Subscribe(UpdateSkybox);

        private void OnDisable() => Realm.Realm.Unsubscribe(UpdateSkybox);


        private void UpdateSkybox(Realm.Realm realm)
        {
            if (realm == null || realm != _realm) return;
            RenderSettings.skybox = _skyboxSettings.SkyboxMaterial;
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = _skyboxSettings.LightColor;
            RenderSettings.ambientIntensity = _skyboxSettings.LightIntensity;
        }

        #if UNITY_EDITOR
        private void Reset() => _realm = Realm.Realm.FromLayer(gameObject.layer);
        #endif
    }
}
