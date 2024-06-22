using System.Linq;
using DualityGame.Realm;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Editor.Realm
{
    [CustomEditor(typeof(RealmVisible))]
    public class RealmVisibleEditor : UnityEditor.Editor
    {
        private RealmVisible _realmVisible;
        private SerializedProperty _realmProperty;

        private VisualElement _misconfiguredLayersTools;
        private VisualElement _realmNotConfiguredTools;

        private void OnEnable()
        {
            _realmVisible = target as RealmVisible;
            _realmProperty = serializedObject.FindProperty(RealmVisible.Fields.Realm);
            Undo.undoRedoPerformed += UpdateTools;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= UpdateTools;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            root.Add(_misconfiguredLayersTools = new VisualElement());
            _misconfiguredLayersTools.Add(new HelpBox
            {
                messageType = HelpBoxMessageType.Error,
                text = "Some layers don't match Realm LevelLayer",
            });
            _misconfiguredLayersTools.Add(new Button(FixMisconfiguredLayers)
            {
                text = "Fix layers"
            });
            _misconfiguredLayersTools.Add(new Button(SelectMisconfiguredLayers)
            {
                text = "Select misconfigured layers"
            });

            root.Add(_realmNotConfiguredTools = new VisualElement());
            _realmNotConfiguredTools.Add(new HelpBox
            {
                messageType = HelpBoxMessageType.Error,
                text = "Realm must be set",
            });
            _realmNotConfiguredTools.Add(new Button(SetRealmAutomatically) { text = "Set Realm automatically" });

            UpdateTools();
            return root;
        }

        private void UpdateTools()
        {
            if (_realmProperty.objectReferenceValue is not DualityGame.Realm.Realm realm)
            {
                _realmNotConfiguredTools.style.display = DisplayStyle.Flex;
                _misconfiguredLayersTools.style.display = DisplayStyle.None;
                return;
            }

            _realmNotConfiguredTools.style.display = DisplayStyle.None;
            var misconfiguredLayers = _realmVisible
                .GetComponentsInChildren<Transform>(true)
                .Any(t => t.gameObject.layer != realm.LevelLayer);
            _misconfiguredLayersTools.style.display = misconfiguredLayers ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void SetRealmAutomatically()
        {
            serializedObject.UpdateIfRequiredOrScript();
            _realmProperty.objectReferenceValue = DualityGame.Realm.Realm.FromLayer(_realmVisible.gameObject.layer);
            serializedObject.ApplyModifiedProperties();
            UpdateTools();
        }

        private void FixMisconfiguredLayers()
        {
            if (_realmProperty.objectReferenceValue is not DualityGame.Realm.Realm realm) return;

            Undo.SetCurrentGroupName("Fix layers");
            var undoGroup = Undo.GetCurrentGroup();
            foreach (var gameObject in _realmVisible
                         .GetComponentsInChildren<Transform>(true)
                         .Select(t => t.gameObject)
                         .Where(gameObject => gameObject.layer != realm.LevelLayer))
            {
                Undo.RecordObject(gameObject, "");
                gameObject.layer = realm.LevelLayer;
                EditorUtility.SetDirty(gameObject);
            }
            Undo.CollapseUndoOperations(undoGroup);
            UpdateTools();
        }

        private void SelectMisconfiguredLayers()
        {
            if (_realmProperty.objectReferenceValue is not DualityGame.Realm.Realm realm) return;

            Selection.objects = _realmVisible
                .GetComponentsInChildren<Transform>(true)
                .Select(t => t.gameObject)
                .Where(gameObject => gameObject.layer != realm.LevelLayer)
                .ToArray();
        }
    }
}
