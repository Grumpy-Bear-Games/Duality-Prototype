using DualityGame.Player;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DualityGame.Editor
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

            if (_spawnPoint.SpawnPointReference.Realm == null) return;
            if (_spawnPoint.gameObject.layer == _spawnPoint.SpawnPointReference.Realm.LevelLayer) return;
            _warnings.Add(new HelpBox
            {
                messageType = HelpBoxMessageType.Error,
                text = "Layer mismatch",
            });
        }
    }
}
