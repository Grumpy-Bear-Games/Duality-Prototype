﻿using DualityGame.Player;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DualityGame.Editor.Player
{
    [CustomEditor(typeof(SpawnPoint))]
    public class SpawnPointEditor : UnityEditor.Editor
    {
        private SpawnPoint _spawnPoint;
        private VisualElement _warnings;
        private SerializedProperty _spawnPointReferenceProperty;

        private void OnEnable()
        {
            _spawnPoint = target as SpawnPoint;
            _spawnPointReferenceProperty = serializedObject.FindProperty($"<{nameof(_spawnPoint.SpawnPointReference)}>k__BackingField");
            Undo.undoRedoPerformed += UpdateWarnings;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= UpdateWarnings;
        }


        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            root.Query<PropertyField>().ForEach(pf => pf.RegisterValueChangeCallback(_ => UpdateWarnings()));

            _warnings = new VisualElement();
            root.Add(_warnings);

            UpdateWarnings();

            return root;
        }

        private void UpdateWarnings()
        {
            _warnings.Clear();

            if (_spawnPoint == null) return;

            if (_spawnPoint.SpawnPointReference == null)
            {
                var button = new Button
                {
                    text = "Create reference"
                };
                button.clicked += CreateReference;
                _warnings.Add(button);
                return;
            }

            if (_spawnPoint.SpawnPointReference.SceneGroup != null)
            {
                var scene = _spawnPoint.gameObject.scene;
                var sceneGroup = SceneGroup.FindFirstWithScene(scene);
                if (sceneGroup != _spawnPoint.SpawnPointReference.SceneGroup)
                {
                    _warnings.Add(new HelpBox
                    {
                        messageType = HelpBoxMessageType.Error,
                        text = "SceneGroup mismatch",
                    });
                    return;
                }
            }

            if (_spawnPoint.Realm == null) return;
            if (_spawnPoint.gameObject.layer == _spawnPoint.Realm.LevelLayer) return;
            _warnings.Add(new HelpBox
            {
                messageType = HelpBoxMessageType.Error,
                text = "Layer mismatch",
            });
        }

        private void CreateReference()
        {
            var path = EditorUtility.SaveFilePanelInProject(
                "Save Spawn Point Reference",
                $"{target.name} Reference.asset",
                "asset",
                "Save Spawn Point Reference"
            );
            if (path.Length == 0) return;

            var spawnPointReference = CreateInstance<SpawnPointReference>();
            AssetDatabase.CreateAsset(spawnPointReference, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            _spawnPointReferenceProperty.objectReferenceValue = spawnPointReference;
            serializedObject.ApplyModifiedProperties();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = spawnPointReference;
        }
    }
}
