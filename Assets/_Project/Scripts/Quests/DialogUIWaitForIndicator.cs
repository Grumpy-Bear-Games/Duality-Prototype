using UnityEngine;

namespace DualityGame.Quests
{
    public class DialogUIWaitForIndicator : MonoBehaviour
    {
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}