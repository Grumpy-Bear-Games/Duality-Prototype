using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    [UxmlObject]
    public partial class ToggleClassBinding: CustomBinding
    {
        [CreateProperty]
        [UxmlAttribute("data-source-path")]
        public string DataSourcePath { get; set; }

        [CreateProperty]
        [UxmlAttribute("class")]
        public string ClassName { get; set; }

        [CreateProperty]
        [UxmlAttribute("inverted")]
        public bool Inverted { get; set; }

        public ToggleClassBinding()
        {
            updateTrigger = BindingUpdateTrigger.OnSourceChanged;
        }

        protected override BindingResult Update(in BindingContext context)
        {
            if (context.dataSource == null)
                return new BindingResult(BindingStatus.Pending, "No data source selected");

            if (string.IsNullOrWhiteSpace(ClassName))
                return new BindingResult(BindingStatus.Pending, "Class name not set");

            if (!PropertyContainer.TryGetValue(context.dataSource, DataSourcePath, out bool value))
                return new BindingResult(BindingStatus.Failure, $"Unable to get property {DataSourcePath} from {context.dataSource}");

            //Debug.Log($"[ToggleClassBinding] {ClassName} -> {Inverted ^ value}");
            context.targetElement.EnableInClassList(ClassName, Inverted ^ value);
            return new BindingResult(BindingStatus.Success, "");
        }
    }
}
