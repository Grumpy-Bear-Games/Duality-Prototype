using System.Collections.Generic;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    [UxmlObject]
    public partial class ListViewCountTracker : CustomBinding
    {
        public static ListViewCountTracker Instance = new();
        private readonly Dictionary<ListView, int> _cachedCount = new();

        protected override void OnActivated(in BindingActivationContext context)
        {
            if (context.targetElement is not ListView listView)
                return;

            // Ensures the refresh will be called on the next update
            _cachedCount[listView] = -1;
        }

        protected override void OnDeactivated(in BindingActivationContext context)
        {
            if (context.targetElement is not ListView listView)
                return;

            _cachedCount.Remove(listView);
        }

        protected override BindingResult Update(in BindingContext context)
        {
            if (context.targetElement is not ListView listView)
                return new BindingResult(BindingStatus.Failure, "'ListViewCountTracker' should only be added to a 'ListView'");

            if (!_cachedCount.TryGetValue(listView, out var previousCount) || previousCount == listView.itemsSource?.Count)
                return new BindingResult(BindingStatus.Failure, "");

            listView.RefreshItems();
            _cachedCount[listView] = listView.itemsSource?.Count ?? -1;

            return new BindingResult(BindingStatus.Success);
        }
    }
}
