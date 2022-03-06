using UnityEngine;

namespace DualityGame.UI
{
    public class MenuInitializer : MonoBehaviour
    {
        [SerializeField] private MenuBase _firstMenu;

        private void Start()
        {
            foreach (var menu in FindObjectsOfType<MenuBase>())
            {
                menu.gameObject.SetActive(false);
            }

            _firstMenu.Open();
        }
    }
}
