using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DualityGame.UI
{
    public class MenuBase : MonoBehaviour, ICancelHandler
    {
        [SerializeField] private Selectable _firstFocused;

        public void Open() => MenuManagement.Show(this, _firstFocused);
        public virtual void Close() => MenuManagement.Back();

        void ICancelHandler.OnCancel(BaseEventData eventData) => Close();
    }
}
