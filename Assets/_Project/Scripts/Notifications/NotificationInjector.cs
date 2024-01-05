using UnityEngine;
using UnityEngine.Playables;

namespace DualityGame.Notifications
{
    public class NotificationInjector : MonoBehaviour, INotificationReceiver
    {
        void INotificationReceiver.OnNotify(Playable origin, INotification notification, object context)
        {
            Debug.Log(notification);
            if (notification is not NotificationMarker notificationMarker) return;
            Debug.Log($"{notificationMarker.Content}", notificationMarker);
            Notifications.Add(notificationMarker.Sprite, notificationMarker.Content);
        }
    }
}
