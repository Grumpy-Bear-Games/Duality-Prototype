using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DualityGame.Utilities.RenderFeatures
{
    internal class RenderToTexturePass : ScriptableRenderPass
    {
        private readonly int _textureNameID;

        private readonly RTHandle _textureHandle;
        private readonly List<ShaderTagId> _shaderTagIds;
        private readonly Material _material;
        private readonly ProfilingSampler _profilingSampler;
        private FilteringSettings _filteringSettings;
        private readonly Color _clearColor;
            
        public RenderToTexturePass(RenderPassEvent renderPassEvent, string textureName, Material material, Color clearColor)
        {
            this.renderPassEvent = renderPassEvent;
            _textureNameID = Shader.PropertyToID(textureName);
            _profilingSampler = new ProfilingSampler($"RenderToTexture({textureName})");
            _textureHandle = RTHandles.Alloc(textureName, name: textureName);
            _shaderTagIds = new List<ShaderTagId>
            {
                new("DepthOnly"),
            };
            _filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
            _material = material;
            _clearColor = clearColor;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(_textureNameID, cameraTextureDescriptor, FilterMode.Point);
            ConfigureTarget(_textureHandle);
            ConfigureClear(ClearFlag.All, _clearColor);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_material == null) return;
            var cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, _profilingSampler))
            {
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                var drawSettings = CreateDrawingSettings(_shaderTagIds, ref renderingData,
                    renderingData.cameraData.defaultOpaqueSortFlags);
                drawSettings.overrideMaterial = _material;
                var rendererListParams = new RendererListParams(renderingData.cullResults, drawSettings, _filteringSettings);
                var rendererList = context.CreateRendererList(ref rendererListParams);
                cmd.DrawRendererList(rendererList);
                //context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref _filteringSettings);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd) => cmd.ReleaseTemporaryRT(_textureNameID);
    }
}
