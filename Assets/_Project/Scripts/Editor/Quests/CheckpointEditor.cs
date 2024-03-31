using DualityGame.Quests;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Editor.Quests
{
    [CustomEditor(typeof(Checkpoint))]
    public class CheckpointEditor : UnityEditor.Editor
    {
        private Checkpoint _checkpoint;
        private Object _mainAsset;

        private void OnEnable()
        {
            _checkpoint = target as Checkpoint;
            _mainAsset = AssetDatabase.IsMainAsset(target) ? null : AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(target));
        }

        public override VisualElement CreateInspectorGUI()
        {
            return EditorApplication.isPlayingOrWillChangePlaymode ? RuntimeMode() : EditorMode();
        }

        private VisualElement RuntimeMode()
        {
            var root = new VisualElement();

            var nameField = new TextField
            {
                label = "Name",
                isDelayed = true,
                value = _checkpoint.name
            };
            nameField.SetEnabled(false);
            root.Add(nameField);

            var reachedToggle = new Toggle
            {
                label = "Reached",
                dataSource = _checkpoint,
            };
            reachedToggle.SetBinding(nameof(Toggle.value), new DataBinding
            {
                dataSourcePath = new PropertyPath(nameof(Checkpoint.Reached)),
            });
            root.Add(reachedToggle);

            return root;
        }

        private VisualElement EditorMode()
        {
            var root = new VisualElement();

            var nameField = new TextField
            {
                label = "Name",
                isDelayed = true
            };
            nameField.RegisterValueChangedCallback(RenameCheckpoint);
            nameField.value = _checkpoint.name;
            root.Add(nameField);

            if (_mainAsset != null)
            {
                var deleteButton = new Button
                {
                    text = "Delete"
                };
                deleteButton.clicked += DeleteNestedCheckpoint;
                root.Add(deleteButton);
            }

            return root;
        }

        private void DeleteNestedCheckpoint()
        {
            AssetDatabase.RemoveObjectFromAsset(_checkpoint);
            EditorUtility.SetDirty(_mainAsset);
            AssetDatabase.SaveAssetIfDirty(_mainAsset);
            Selection.activeObject = _mainAsset;
        }

        private void RenameCheckpoint(ChangeEvent<string> evt)
        {
            //EditorUtility.SetDirty(_checkpoint);
            if (_mainAsset == null)
            {
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(_checkpoint), evt.newValue);
            }
            else
            {
                _checkpoint.name = evt.newValue;
                EditorUtility.SetDirty(_mainAsset);
                AssetDatabase.SaveAssetIfDirty(_mainAsset);
            }

        }
    }
}
