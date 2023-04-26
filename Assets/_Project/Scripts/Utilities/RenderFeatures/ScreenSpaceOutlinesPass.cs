using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DualityGame.Utilities.RenderFeatures
{
    internal class ScreenSpaceOutlinesPass : ScriptableRenderPass
    {
        private readonly int _textureNameID;
        private readonly RTHandle _textureHandle;

        public RTHandle _cameraColorTarget { get; set; }
        private readonly ProfilingSampler _profilingSampler = new ("ScreenSpaceOutlines");
        private readonly Material _screenSpaceOutlineMaterial;
        
        

        public ScreenSpaceOutlinesPass(RenderPassEvent renderPassEvent, Material screenSpaceOutlineMaterial, ShaderSettings shaderSettings, string textureName)
        {
            this.renderPassEvent = renderPassEvent;
            _screenSpaceOutlineMaterial = screenSpaceOutlineMaterial;
            
            _screenSpaceOutlineMaterial.SetFloat(ShaderSettings.NormalThresholdProperty, shaderSettings.ViewSpaceNormalThreshold);
            _screenSpaceOutlineMaterial.SetFloat(ShaderSettings.FaceIdThresholdProperty, shaderSettings.FaceIdThreshold);
            _screenSpaceOutlineMaterial.SetFloat(ShaderSettings.DepthThresholdProperty, shaderSettings.DepthThreshold);
            
            _textureNameID = Shader.PropertyToID(textureName);
            _textureHandle = RTHandles.Alloc(textureName, name: textureName);
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(_textureNameID, cameraTextureDescriptor, FilterMode.Point);
            ConfigureTarget(_textureHandle);
            ConfigureClear(ClearFlag.All, Color.clear);
        }            

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_screenSpaceOutlineMaterial == null) return;
            if (_cameraColorTarget == null) return;
                
            var cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, _profilingSampler))
            {
                Blitter.BlitCameraTexture(cmd, _cameraColorTarget, _textureHandle, _screenSpaceOutlineMaterial, 0);
            }
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
            
        public override void OnCameraCleanup(CommandBuffer cmd) => cmd.ReleaseTemporaryRT(_textureNameID);
       
        [Serializable]
        public sealed class ShaderSettings
        {
            public static readonly int NormalThresholdProperty = Shader.PropertyToID("_Normal_Threshold");
            public static readonly int FaceIdThresholdProperty = Shader.PropertyToID("_Object_ID_Threshold");
            public static readonly int DepthThresholdProperty = Shader.PropertyToID("_Depth_Threshold");

            [field: SerializeField][field: Range(0.001f, 10f)]
            public float ViewSpaceNormalThreshold { get; private set; }  = 0.69f;

            [field: SerializeField][field: Range(0.001f, 6f)]
            public float FaceIdThreshold { get; private set; } = 0.24f;
            
            [field: SerializeField][field: Range(0.0001f, 0.001f)]
            public float DepthThreshold { get; private set; } = 0.0003f;
        }
    }
}
