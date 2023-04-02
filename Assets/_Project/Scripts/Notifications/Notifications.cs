using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Notifications
{
    public static class Notifications
    {
        private static readonly Queue<INotificationItem> _items = new();
        public static int Count => _items.Count;
        public static event Action OnItemsAdded;

        public static INotificationItem Pop() => _items.Dequeue();


        public static void Add(string content) => Add(null, content);
        public static void Add(Sprite sprite, string content) => Add(new GenericNotificationItem(sprite, content));

        public static void Add(INotificationItem notification)
        {
            _items.Enqueue(notification);
            OnItemsAdded?.Invoke();
        }

        public sealed class GenericNotificationItem: INotificationItem
        {
            private readonly Sprite _sprite;
            private readonly string _content;

            public GenericNotificationItem(Sprite sprite, string content)
            {
                this._sprite = sprite;
                _content = content;
            }

            VisualElement INotificationItem.CreateVisualElement() => new UI.NotificationItem(_sprite, _content);
        }
    }
}
