using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DualityGame.Notifications
{
    public class NotificationMarker : Marker, INotification
    {
        [field:SerializeField] public string Content { get; private set; }
        [field:SerializeField] public Sprite Sprite { get; private set; }

        public PropertyName id => new();
    }
}
