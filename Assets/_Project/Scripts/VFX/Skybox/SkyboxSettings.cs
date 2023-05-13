using UnityEngine;

namespace DualityGame.VFX.Skybox
{
    [CreateAssetMenu(fileName = "Skybox Settings", menuName = "Duality/Skybox Settings", order = 0)]
    public class SkyboxSettings : ScriptableObject
    {
        [field: SerializeField] public Material SkyboxMaterial { get; private set;  }
        [field: SerializeField] public Color LightColor { get; private set;  }
        [field: SerializeField][field: Range(0f, 10f)] public float LightIntensity { get; private set; } = 1f;
    }
}
