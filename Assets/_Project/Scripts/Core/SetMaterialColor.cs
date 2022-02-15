using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DualityGame.Core{

    [Category("Duality")]
    [Description("Set material color for a single instance")]
    public class SetMaterialColor : ActionTask
    {
        [RequiredField] public BBParameter<Renderer> renderer;
        [RequiredField] public BBParameter<string> propertyName = "_Color";
        [RequiredField][ColorUsage(true, true)] public BBParameter<Color> color;

        private MaterialPropertyBlock _materialPropertyBlock;

        protected override string info => "Set material color";
		
        protected override void OnExecute()
        {
            _materialPropertyBlock ??= new MaterialPropertyBlock();
            _materialPropertyBlock.SetColor(propertyName.value, color.value);
            renderer.value.SetPropertyBlock(_materialPropertyBlock);
            EndAction(true);
        }

    }
}
