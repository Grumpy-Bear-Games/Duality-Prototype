using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Editor.Utilities
{
    public class PrefabReplacer : EditorWindow
    {
        [MenuItem("Duality Game/Refactoring/Replace prefabs in scene")]
        private static void ShowWindow()
        {
            var window = GetWindow<PrefabReplacer>();
            window.titleContent = new GUIContent("Replace prefabs in scene");
            window.Show();
        }

        private GameObject[] _prefabInstances;
        private ObjectField _oldPrefab;
        private ObjectField _newPrefab;
        private Label _oldPrefabPath;
        private Label _newPrefabPath;
        private Button _replaceButton;

        private void CreateGUI()
        {
            _oldPrefab = new ObjectField("Old prefab")
            {
                allowSceneObjects = false,
                objectType = typeof(GameObject)
            };
            _newPrefab = new ObjectField("New prefab")
            {
                allowSceneObjects = false,
                objectType = typeof(GameObject)
            };

            _oldPrefabPath = new Label();
            _newPrefabPath = new Label();

            _replaceButton = new Button(Replace){ text = "Replace" };
            _replaceButton.SetEnabled(false);

            rootVisualElement.Add(_oldPrefab);
            rootVisualElement.Add(_oldPrefabPath);
            rootVisualElement.Add(_newPrefab);
            rootVisualElement.Add(_newPrefabPath);
            rootVisualElement.Add(_replaceButton);
            
            _oldPrefab.RegisterValueChangedCallback(_ =>
            {
                UpdateButton();
                _oldPrefabPath.text = _oldPrefab.value != null ? PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(_oldPrefab.value as GameObject) : "";
            });

            _newPrefab.RegisterValueChangedCallback(_ =>
            {
                UpdateButtonState();
                _newPrefabPath.text = _newPrefab.value != null ? PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(_newPrefab.value as GameObject) : "";
            });
        }

        private void UpdateButtonState() => _replaceButton.SetEnabled(_oldPrefab.value != null && _newPrefab.value != null);

        private void UpdateButton()
        {
            if (_oldPrefab.value == null)
            {
                _replaceButton.text = "Replace";
            }
            else
            {
                var instances = PrefabUtility.FindAllInstancesOfPrefab(_oldPrefab.value as GameObject);
                _replaceButton.text = $"Replace {instances.Count()} instances of {_oldPrefab.value.name}";
            }

            UpdateButtonState();
        }

        private void Replace()
        {
            var instances = PrefabUtility.FindAllInstancesOfPrefab(_oldPrefab.value as GameObject);
            PrefabUtility.ReplacePrefabAssetOfPrefabInstances(instances, _newPrefab.value as GameObject, InteractionMode.UserAction);
            UpdateButton();
        }
    }
}
