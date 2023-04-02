using System.Collections;
using System.Linq;
using DualityGame.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Notifications.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class NotificationUI : MonoBehaviour
    {
        [SerializeField] private StyleSheet _styleSheet;
        [SerializeField] private int _maxItemsOnScreen = 5;
        [SerializeField] private float _disappearAfterSeconds = 3f;

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
                StartCoroutine(NotificationLifeCycle_CO(visualElement));
            }
        }

        private IEnumerator NotificationLifeCycle_CO(VisualElement notification)
        {
            using var transitionMonitor = new TransitionMonitor(notification);
            _notificationsArea.Add(notification);
            
            notification.AddToClassList("enter");
            yield return transitionMonitor.WaitUntilDone();
            notification.RemoveFromClassList("enter");
            yield return transitionMonitor.WaitUntilDone();

            while (_notificationsArea.Children().First() != notification)
            {
                yield return null;
            }

            yield return new WaitForSeconds(_disappearAfterSeconds);
            
            notification.AddToClassList("leave");
            yield return transitionMonitor.WaitUntilDone();
            
            _notificationsArea.Remove(notification);
            
            FillFromQueue();
        }
    }
}
