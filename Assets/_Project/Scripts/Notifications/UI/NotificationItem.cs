using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Notifications.UI
{
    public class NotificationItem: VisualElement
    {
        private const string UssBaseClass = "notification-item";

        public NotificationItem(string content) : this(null, content) { }

        public NotificationItem(Sprite sprite, string content)
        {
            if (sprite != null)
            {
                hierarchy.Add(new VisualElement
                {
                    style =
                    {
                        backgroundImage = new StyleBackground(sprite)
                    },
                    name = "Icon"
                });
            }

            hierarchy.Add(new Label
            {
                text = content,
                name = "Content"
            });
            AddToClassList(UssBaseClass);
        }
    }
}
