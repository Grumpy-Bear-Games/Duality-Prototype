using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DualityGame.Utilities.RenderFeatures
{
    [VolumeComponentMenu("Duality/Screen Space Outlines")]
    [SupportedOnRenderPipeline(typeof(UniversalRenderPipelineAsset))]
    public class ScreenSpaceOutlinesComponent : VolumeComponent, IPostProcessComponent
    {
        [Header("Edge Detection")]
        [SerializeField] private ClampedFloatParameter _viewSpaceNormalThreshold = new(0.69f, 0.001f, 10f);
        [SerializeField] private ClampedFloatParameter _faceIdThreshold = new(0.24f, 0.001f, 6f);
        [SerializeField] private ClampedFloatParameter _depthThreshold = new(0.0003f, 0.0001f, 0.001f);

        [Header("Outline Lines")]
        [SerializeField] private ClampedFloatParameter _outlineKernelSize = new(0, 0f, 10f);
        [SerializeField] private ColorParameter _outlineColor = new ColorParameter(Color.black, true, false, true);

        [Header("Outline Noise")]
        [SerializeField] private ClampedFloatParameter _noiseScale = new(4.9f, 0f, 20f);
        [SerializeField] private ClampedFloatParameter _noiseStrength = new(1.8f, 0f, 20f);
        
        public float ViewSpaceNormalThreshold => _viewSpaceNormalThreshold.value;
        public float FaceIdThreshold => _faceIdThreshold.value;
        public float DepthThreshold => _depthThreshold.value;

        public float OutlineKernelSize => _outlineKernelSize.value;
        public Color OutlineColor => _outlineColor.value;
        public float NoiseScale => _noiseScale.value;
        public float NoiseStrength => _noiseStrength.value;
        
        bool IPostProcessComponent.IsActive() => true;
        bool IPostProcessComponent.IsTileCompatible() => false;
    }
}
