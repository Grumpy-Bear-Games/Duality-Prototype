using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DualityGame.Notifications.Tasks
{
    [Category("Duality")]
    [Description("Add Notification")]
    public class AddNotification : ActionTask
    {
        [RequiredField] public BBParameter<string> _content;
        public BBParameter<Sprite> _sprite;

        protected override string info => _content.isNoneOrNull ? "(Please specify content)" : $"Notification: \"{_content.value}\"";

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            Notifications.Add(_sprite.value, _content.value);
            EndAction(true);
        }
    }
}
