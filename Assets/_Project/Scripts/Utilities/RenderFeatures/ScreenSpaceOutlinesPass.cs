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
        private readonly Material _detectOutlineMaterial;
        private readonly Material _drawOutlineMaterial; 

        public ScreenSpaceOutlinesPass(RenderPassEvent renderPassEvent, Material detectOutlineMaterial, Material drawOutlineMaterial, string textureName)
        {
            this.renderPassEvent = renderPassEvent;
            _detectOutlineMaterial = detectOutlineMaterial;
            _drawOutlineMaterial = drawOutlineMaterial;
            
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
            if (_detectOutlineMaterial == null) return;
            if (_cameraColorTarget == null) return;

            var component = VolumeManager.instance.stack.GetComponent<ScreenSpaceOutlinesComponent>();

            _detectOutlineMaterial.SetFloat(ShaderSettings.NormalThresholdProperty, component.ViewSpaceNormalThreshold);
            _detectOutlineMaterial.SetFloat(ShaderSettings.FaceIdThresholdProperty, component.FaceIdThreshold);
            _detectOutlineMaterial.SetFloat(ShaderSettings.DepthThresholdProperty, component.DepthThreshold);
            
            _drawOutlineMaterial.SetFloat(ShaderSettings.OutlineKernelSizeProperty, component.OutlineKernelSize);
            _drawOutlineMaterial.SetColor(ShaderSettings.OutlineColorProperty, component.OutlineColor);
            _drawOutlineMaterial.SetFloat(ShaderSettings.NoiseScaleProperty, component.NoiseScale);
            _drawOutlineMaterial.SetFloat(ShaderSettings.NoiseStrengthProperty, component.NoiseStrength);
                
            var cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, _profilingSampler))
            {
                Blitter.BlitCameraTexture(cmd, _cameraColorTarget, _textureHandle, _detectOutlineMaterial, 0);
                Blitter.BlitCameraTexture(cmd, _cameraColorTarget, _cameraColorTarget, _drawOutlineMaterial, 0);
            }
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
            
        public override void OnCameraCleanup(CommandBuffer cmd) => cmd.ReleaseTemporaryRT(_textureNameID);

        private sealed class ShaderSettings
        {
            public static readonly int NormalThresholdProperty = Shader.PropertyToID("_Normal_Threshold");
            public static readonly int FaceIdThresholdProperty = Shader.PropertyToID("_Object_ID_Threshold");
            public static readonly int DepthThresholdProperty = Shader.PropertyToID("_Depth_Threshold");
            
            public static readonly int OutlineKernelSizeProperty = Shader.PropertyToID("_Outline_Kernel_Size");
            public static readonly int OutlineColorProperty = Shader.PropertyToID("_Outline_Color");
            public static readonly int NoiseScaleProperty = Shader.PropertyToID("_Noise_Scale");
            public static readonly int NoiseStrengthProperty = Shader.PropertyToID("_Noise_Strength");
        }
    }
}
