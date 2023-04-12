using UnityEngine;
using UnityEngine.Rendering;

namespace DualityGame.Realm
{
    public class SkyboxManager : MonoBehaviour
    {
        private void OnEnable() => Realm.Subscribe(UpdateSkybox);

        private void OnDisable() => Realm.Unsubscribe(UpdateSkybox);


        private void UpdateSkybox(Realm realm)
        {
            if (realm == null) return;
            RenderSettings.skybox = realm.SkyboxMaterial;
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = realm.LightColor;
            RenderSettings.ambientIntensity = realm.LightIntensity;
        }
    }
}
