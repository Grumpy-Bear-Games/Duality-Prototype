using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Player.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class WarpUI : MonoBehaviour
    {

        private VisualElement _warpIcon;
        private void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _warpIcon = root.Q<VisualElement>("Warp");
        }

        private void OnEnable() => WarpController.State.Subscribe(UpdateWarpIcon);

        private void OnDisable() => WarpController.State.Unsubscribe(UpdateWarpIcon);

        private void UpdateWarpIcon(WarpController.WarpState warpState)
        {
            _warpIcon.EnableInClassList("Disabled", warpState != WarpController.WarpState.CanWarp);
        }
    }
}