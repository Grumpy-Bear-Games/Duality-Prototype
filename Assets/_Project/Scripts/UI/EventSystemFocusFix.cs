using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    [RequireComponent(typeof(EventSystem))]
    public class EventSystemFocusFix : MonoBehaviour
    {
        private void Start()
        {
            var panelEventHandler = GetComponentInChildren<PanelEventHandler>();
            Assert.IsNotNull(panelEventHandler);
            GetComponent<EventSystem>().SetSelectedGameObject(panelEventHandler.gameObject);
        }
    }
}
