using UnityEngine;

namespace DualityGame.DialogSystem.UI
{
    public class WaitForInputIndicator : MonoBehaviour
    {
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}
