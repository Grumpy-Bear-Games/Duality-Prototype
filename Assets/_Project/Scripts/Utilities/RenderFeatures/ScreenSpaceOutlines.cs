using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DualityGame.Utilities.RenderFeatures
{
    public class ScreenSpaceOutlines : ScriptableRendererFeature
    {
        private const string ViewSpaceNormalsTexture = "_ViewSpaceNormals";
        private const string FaceIdTexture = "_ObjectId";
        private const string TextureName = "_ScreenSpaceOutlines";

        [Header("Edge Detection")]
        [SerializeField] private ScreenSpaceOutlinesPass.ShaderSettings _edgeDetectionShaderSettings = new();
        [SerializeField] private RenderPassEvent _renderPassEvent;

        [Header("Shaders")]
        [SerializeField] private Shader _viewSpaceNormalsShader;
        [SerializeField] private Shader _faceIdsShader;
        [SerializeField] private Shader _detectOutlinesShader;

        private RenderToTexturePass _viewSpaceNormalsPass;
        private Material _viewSpaceNormalsMaterial;
        private RenderToTexturePass _faceIdPass;
        private Material _faceIdMaterial;
        
        private ScreenSpaceOutlinesPass _screenSpaceOutlinesPass;
        private Material _screenSpaceOutlineMaterial;

        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        {
            _screenSpaceOutlinesPass.ConfigureInput(ScriptableRenderPassInput.Color);
            _screenSpaceOutlinesPass._cameraColorTarget=renderer.cameraColorTargetHandle;
        }

        public override void Create()
        {
            _viewSpaceNormalsMaterial = CoreUtils.CreateEngineMaterial(_viewSpaceNormalsShader);
            _viewSpaceNormalsPass = new RenderToTexturePass(RenderPassEvent.AfterRenderingOpaques,
                ViewSpaceNormalsTexture, _viewSpaceNormalsMaterial, Color.clear);
            
            _faceIdMaterial = CoreUtils.CreateEngineMaterial(_faceIdsShader);
            _faceIdPass = new RenderToTexturePass(RenderPassEvent.AfterRenderingOpaques,
                FaceIdTexture, _faceIdMaterial, Color.clear);

            _screenSpaceOutlineMaterial = CoreUtils.CreateEngineMaterial(_detectOutlinesShader);
            _screenSpaceOutlinesPass = new ScreenSpaceOutlinesPass(_renderPassEvent, _screenSpaceOutlineMaterial, _edgeDetectionShaderSettings, TextureName);
        }

        protected override void Dispose(bool disposing)
        {
            CoreUtils.Destroy(_viewSpaceNormalsMaterial);
            CoreUtils.Destroy(_faceIdMaterial);
            CoreUtils.Destroy(_screenSpaceOutlineMaterial);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(_viewSpaceNormalsPass);
            renderer.EnqueuePass(_faceIdPass);
            renderer.EnqueuePass(_screenSpaceOutlinesPass);
        }
    }
}
