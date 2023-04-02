using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Notifications.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class NotificationUI : MonoBehaviour
    {
        [SerializeField] private StyleSheet _styleSheet;
        [SerializeField] private int _maxItemsOnScreen = 5;
        [SerializeField] private int _fadeOutAfterMs = 3000;

        private VisualElement _notificationsArea;

        private void Awake()
        {
            _notificationsArea = GetComponent<UIDocument>().rootVisualElement;
            _notificationsArea.styleSheets.Add(_styleSheet);
        }

        private void OnEnable()
        {
            Notifications.OnItemsAdded += FillFromQueue;
            FillFromQueue();
        }

        private void OnDisable() => Notifications.OnItemsAdded -= FillFromQueue;

        private void FillFromQueue()
        {
            while (_notificationsArea.childCount < _maxItemsOnScreen && Notifications.Count > 0)
            {
                var item = Notifications.Pop();
                var visualElement = item.CreateVisualElement();
                visualElement.AddToClassList("pop-in");
                visualElement.schedule.Execute(() => visualElement.RemoveFromClassList("pop-in")).ExecuteLater(10);
                _notificationsArea.Add(visualElement);
                if (_notificationsArea.childCount == 1) PopTopmostItem();
            }
        }

        private void PopTopmostItem()
        {
            if (_notificationsArea.childCount == 0) return;
            var top = _notificationsArea.Children().First();
            top.schedule.Execute(() =>
            {
                top.RegisterCallback<TransitionEndEvent>(_ =>
                {
                    _notificationsArea.Remove(top);
                    if (_notificationsArea.childCount > 0) PopTopmostItem();
                    FillFromQueue();
                });
                top.AddToClassList("fade-out");
            }).ExecuteLater(_fadeOutAfterMs);
        }
    }
}
