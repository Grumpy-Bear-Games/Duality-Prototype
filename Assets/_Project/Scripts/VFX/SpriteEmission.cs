using System;
using UnityEngine;

namespace DualityGame.VFX
{
    [ExecuteAlways]
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteEmission : MonoBehaviour
    {
        private static readonly int EmissionTex = Shader.PropertyToID("_EmissionTex");
        private static readonly int EmissionStrength = Shader.PropertyToID("_EmissionStrength");

        [Range(0f, 10f)]
        [SerializeField] private float _emissionStrength = 1;
        [SerializeField] private Sprite _emissionSprite;

        private void Awake() => UpdateMaterial();

        private void OnEnable() => UpdateMaterial();

        private void OnDisable() => UpdateMaterial();

        private void OnDestroy() => GetComponent<SpriteRenderer>().SetPropertyBlock(null);

        private void UpdateMaterial()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();

            if (!enabled)
            {
                spriteRenderer.SetPropertyBlock(null);
                return;
            }

            var block = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(block);

            block.SetTexture(EmissionTex, _emissionSprite?.texture);
            block.SetFloat(EmissionStrength, _emissionStrength);

            spriteRenderer.SetPropertyBlock(block);
        }

        private void OnValidate() => UpdateMaterial();
    }
}
