using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Editor.Utilities
{
    public class MaterialReplacer : EditorWindow
    {
        [MenuItem("Duality Game/Refactoring/Replace materials")]
        private static void ShowWindow()
        {
            var window = GetWindow<MaterialReplacer>();
            window.titleContent = new GUIContent("Replace materials");
            window.Show();
        }

        private ObjectField _oldMaterial;
        private ObjectField _newMaterial;
        private Button _replaceButton;

        private void CreateGUI()
        {
            var root = rootVisualElement;
            _oldMaterial = new ObjectField
            {
                label = "Old material",
                objectType = typeof(Material)
            };
            _oldMaterial.RegisterValueChangedCallback(_ => OnSelectionChange());
            _newMaterial = new ObjectField()
            {
                label = "New material",
                objectType = typeof(Material)
            };
            _newMaterial.RegisterValueChangedCallback(_ => OnSelectionChange());
            _replaceButton = new Button(ReplaceMaterials)
            {
                text = "Replace"
            };
            root.Add(_oldMaterial);
            root.Add(_newMaterial);
            root.Add(_replaceButton);
            OnSelectionChange();
        }

        private void OnSelectionChange()
        {
            var count = 0;
            foreach (var renderer in Selection.GetFiltered<Renderer>(SelectionMode.Deep))
            {
                if (_oldMaterial.value != null && renderer.sharedMaterial != _oldMaterial.value) continue;
                count++;
            }

            _replaceButton.text = $"Replace in {count} prefabs";
            _replaceButton.SetEnabled(_newMaterial.value != null && count > 0);
        }

        private void ReplaceMaterials()
        {
            foreach (var renderer in Selection.GetFiltered<Renderer>(SelectionMode.Deep))
            {
                if (_oldMaterial.value != null && renderer.sharedMaterial != _oldMaterial.value) continue;
                renderer.sharedMaterial = _newMaterial.value as Material;
                EditorUtility.SetDirty(renderer);
            }
            OnSelectionChange();
        }
    }
}
