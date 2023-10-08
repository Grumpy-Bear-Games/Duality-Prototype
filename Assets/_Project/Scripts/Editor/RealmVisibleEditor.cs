using System;
using System.Linq;
using DualityGame.Realm;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Editor
{
    [CustomEditor(typeof(RealmVisible))]
    public class RealmVisibleEditor : UnityEditor.Editor
    {
        private RealmVisible _realmVisible;
        private SerializedProperty _realmProperty;

        private VisualElement _tools;

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
            _tools = new VisualElement();
            root.Add(_tools);

            UpdateTools();
            return root;
        }

        private void UpdateTools()
        {
            _tools.Clear();
            if (_realmProperty.objectReferenceValue is Realm.Realm realm)
            {
                var misconfiguredLayers = _realmVisible
                    .GetComponentsInChildren<Transform>(true)
                    .Any(t => t.gameObject.layer != realm.LevelLayer);
                if (!misconfiguredLayers) return;
                _tools.Add(new HelpBox
                {
                    messageType = HelpBoxMessageType.Error,
                    text = "Some layers don't match Realm LevelLayer",
                });
                _tools.Add(new Button(() =>
                {
                    Undo.SetCurrentGroupName("Fix layers");
                    int undoGroup = Undo.GetCurrentGroup();
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
                })
                {
                    text = "Fix layers"
                });
            }
            else
            {
                _tools.Add(new HelpBox
                {
                    messageType = HelpBoxMessageType.Error,
                    text = "Realm must be set",
                });
                _tools.Add(new Button(() =>
                {
                    serializedObject.UpdateIfRequiredOrScript();
                    _realmProperty.objectReferenceValue = Realm.Realm.FromLayer(_realmVisible.gameObject.layer);
                    serializedObject.ApplyModifiedProperties();
                    UpdateTools();
                })
                {
                    text = "Set Realm automatically"
                });
            }
        }
    }
}
