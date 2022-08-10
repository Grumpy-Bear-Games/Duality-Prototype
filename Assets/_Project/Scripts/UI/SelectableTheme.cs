using UnityEngine;
using UnityEngine.UI;

namespace DualityGame.UI
{
    public class SelectableTheme : MonoBehaviour
    {
        [SerializeField] private ColorBlock _colorBlock;

        private void Awake() => UpdateColors();

        private void UpdateColors()
        {
            foreach (var selectable in gameObject.GetComponentsInChildren<Selectable>())
            {
                selectable.colors = _colorBlock;
            }
        }

        private void OnValidate() => UpdateColors();
    }
}
