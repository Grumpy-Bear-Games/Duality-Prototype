using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    [UxmlElement]
    public partial class CollectionView: VisualElement
    {
        #region Private fields
        private VisualTreeAsset _itemTemplate = null;
        private IList _itemsSource;
        private List<VisualElement> _children;
        private bool _refreshQueued;

        #endregion

        #region Attributes
        [UxmlAttribute("item-template")]
        public VisualTreeAsset ItemTemplate
        {
            get => _itemTemplate;
            set
            {
                _itemTemplate = value;
                RefreshItems();
            }
        }
        #endregion

        [CreateProperty]
        public IList ItemsSource
        {
            get => _itemsSource;
            set
            {
                if (_itemsSource == value) return;
                _itemsSource = value;
                RefreshItems();
            }
        }

        public CollectionView()
        {
            RegisterCallback<AttachToPanelEvent>(_ => QueueRefreshItems());
        }

        private void QueueRefreshItems()
        {
            if (_refreshQueued) return;
            schedule.Execute(() =>
            {
                _refreshQueued = false;
                RefreshItems();
            });
        }

        private void RefreshItems()
        {
            // Implement less naive approach.
            Clear();
            if (_itemsSource == null) return;
            if (_itemTemplate == null) return;


            foreach (var item in _itemsSource)
            {
                var element = _itemTemplate.CloneTree();
                element.name = "Item";
                element.AddToClassList("collection-view__item");
                Add(element);
                element.dataSource = item;
            }
        }
    }
}
