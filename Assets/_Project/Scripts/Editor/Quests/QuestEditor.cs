using System.Collections.Generic;
using System.Linq;
using DualityGame.Quests;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DualityGame.Editor.Quests
{
    [CustomEditor(typeof(Quest))]
    public class QuestEditor : UnityEditor.Editor
    {
        private readonly List<Checkpoint> _checkpoints = new();

        private void RefreshCheckpoints()
        {
            var path = AssetDatabase.GetAssetPath(target);
            var checkpointsInAsset = AssetDatabase.LoadAllAssetRepresentationsAtPath(path)
                .Select(obj => obj as Checkpoint)
                .Where(obj => obj != null);

            _checkpoints.Clear();
            _checkpoints.AddRange(checkpointsInAsset);
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            RefreshCheckpoints();

            var checkpointsList = new ListView
            {
                showAddRemoveFooter = true,
                headerTitle = "Checkpoints",
                showFoldoutHeader = true,
                showBoundCollectionSize = false,
                itemsSource = _checkpoints,
                showBorder = true,
                showAlternatingRowBackgrounds = AlternatingRowBackground.ContentOnly
            };

            checkpointsList.makeItem += CheckpointsMakeItem;
            checkpointsList.bindItem += CheckpointsBindItem;
            checkpointsList.itemsAdded += AddCheckpoints;
            checkpointsList.itemsRemoved += RemoveCheckpoints;

            root.Add(checkpointsList);

            return root;
        }

        private void AddCheckpoints(IEnumerable<int> indicies)
        {
            foreach (var index in indicies)
            {
                var checkpoint = CreateInstance<Checkpoint>();
                checkpoint.name = $"<new checkpoint {index}>";
                AssetDatabase.AddObjectToAsset(checkpoint, target);
            }
            AssetDatabase.SaveAssetIfDirty(target);
            RefreshCheckpoints();
        }

        private void RemoveCheckpoints(IEnumerable<int> indicies)
        {
            EditorUtility.SetDirty(target);
            foreach (var index in indicies)
            {
                var checkpoint = _checkpoints[index];
                AssetDatabase.RemoveObjectFromAsset(checkpoint);
                DestroyImmediate(checkpoint);
            }
            AssetDatabase.SaveAssetIfDirty(target);
        }

        private void CheckpointsBindItem(VisualElement checkpointElement, int index)
        {
            var checkpoint = _checkpoints[index];
            if (checkpoint == null) return;
            checkpointElement.Q<TextField>().SetValueWithoutNotify(checkpoint.name);
            checkpointElement.Q<TextField>().userData = checkpoint;
        }

        private VisualElement CheckpointsMakeItem()
        {
            var root = new VisualElement();
            var nameField = new TextField
            {
                label = "Name",
                isDelayed = true
            };
            nameField.RegisterValueChangedCallback(RenameCheckpoint);
            root.Add(nameField);
            return root;
        }

        private void RenameCheckpoint(ChangeEvent<string> evt)
        {
            if (evt.target is not TextField textField) return;
            if (textField.userData is not Checkpoint checkpoint) return;
            checkpoint.name = evt.newValue;
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssetIfDirty(target);
        }
    }
}
