using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    [UxmlObject] // shows up in Builder → Add Binding
    public partial class SelectionToDataSourceBinding : CustomBinding
    {
        // Name of the element (in the same panel) that should receive the selected item as its data source.
        [UxmlAttribute("target-name")]
        public string TargetName { get; set; }

        // If true, clear the target's dataSourcePath when assigning the new dataSource.
        [UxmlAttribute("clear-path")]
        public bool ClearPath { get; set; } = true;

        [UxmlAttribute("hide-when-empty")]
        public bool HideWhenEmpty { get; set; } = true;

        private ListView _listView;
        private VisualElement _target;

        protected override void OnActivated(in BindingActivationContext ctx)
        {
            _listView = ctx.targetElement as ListView;
            if (_listView == null) return; // this binding must be attached to a ListView

            _listView.selectionChanged += OnSelectionChanged;
            _listView.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);

            ApplySelection();
        }


         // Not strictly needed for this binding, but rebasing selection on data-source changes is harmless.
        protected override void OnDataSourceChanged(in DataSourceContextChanged _) => ApplySelection();

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            TryResolveTarget((VisualElement)evt.target);
            ApplySelection();
        }


        protected override void OnDeactivated(in BindingActivationContext ctx)
        {
            if (_listView != null)
            {
                _listView.selectionChanged -= OnSelectionChanged;
                _listView?.UnregisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            }

            _listView = null;
            _target = null;
        }

        private void OnSelectionChanged(IEnumerable<object> _) => ApplySelection();

        private void ApplySelection()
        {
            if (_listView == null)  return;

            // Prefer selectedItem when available; fall back to first selected element if needed.
            var selected = _listView.selectedItem;
            if (selected == null)
            {
                // In case selectedItem is null but selection exists (older Unity versions edge case)
                foreach (var obj in _listView.selectedItems)
                {
                    selected = obj;
                    break;
                }
            }

            if (_target == null) TryResolveTarget(_listView);

            if (_target == null) return;
            _target.dataSource = selected;
            if (HideWhenEmpty)
            {
                _target.style.display = selected is null ? DisplayStyle.None : StyleKeyword.Undefined;
            }


            if (ClearPath) _target.dataSourcePath = default; // let bindings read directly from the selected item
        }

        private void TryResolveTarget(VisualElement ve)
        {
            if (string.IsNullOrEmpty(TargetName)) return;
            var root = ve.panel?.visualTree;
            _target = root?.Q<VisualElement>(TargetName);
        }
    }
}
