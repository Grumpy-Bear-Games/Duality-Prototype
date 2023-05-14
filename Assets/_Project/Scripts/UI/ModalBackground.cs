using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class ModalBackground : MonoBehaviour
    {
        private VisualElement _background;

        private void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _background = root.Q<VisualElement>(className: "ModalBackground");
        }

        private void OnEnable() => _background.AddToClassList("Shown");
        private void OnDisable() => _background.RemoveFromClassList("Shown");
    }
}
