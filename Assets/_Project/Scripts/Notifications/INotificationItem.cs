using UnityEngine.UIElements;

namespace DualityGame.Notifications
{
    public interface INotificationItem
    {
        VisualElement CreateVisualElement();
    }
}
