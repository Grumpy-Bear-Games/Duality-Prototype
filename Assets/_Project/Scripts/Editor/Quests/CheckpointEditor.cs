using DualityGame.Quests;
using UnityEditor;
using UnityEditor.UIElements;
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
            var root = new VisualElement();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            var nameField = new TextField
            {
                label = "Name",
                isDelayed = true
            };
            nameField.RegisterValueChangedCallback(RenameCheckpoint);
            nameField.value = _checkpoint.name;
            root.Add(nameField);

            var deleteButton = new Button
            {
                text = "Delete"
            };
            deleteButton.clicked += DeleteCheckpoint;
            root.Add(deleteButton);
            return root;
        }

        private void DeleteCheckpoint()
        {
            if (_mainAsset == null)
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(_checkpoint));
                DestroyImmediate(_checkpoint, true);
                Selection.activeObject = null;
            }
            else
            {
                AssetDatabase.RemoveObjectFromAsset(_checkpoint);
                EditorUtility.SetDirty(_mainAsset);
                AssetDatabase.SaveAssetIfDirty(_mainAsset);
                Selection.activeObject = _mainAsset;
            }
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
