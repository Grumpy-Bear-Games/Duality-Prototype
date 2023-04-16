using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace DualityGame.Utilities.RenderFeatures
{
    public class RenderToTexture : ScriptableRendererFeature
    {
        [SerializeField] private Color _clearColor = Color.clear;

        [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        [SerializeField] private Material _material;
        [SerializeField] private string _textureName = "_ViewSpaceNormals";

        private RenderToTexturePass _renderToTexturePass;


        public override void Create()
        {
            _renderToTexturePass = new RenderToTexturePass(_renderPassEvent, _textureName, _material, _clearColor);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(_renderToTexturePass);
        }
    }
}
