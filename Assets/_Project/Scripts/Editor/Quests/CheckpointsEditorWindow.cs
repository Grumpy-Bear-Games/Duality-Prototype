using System.Linq;
using DualityGame.Quests;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Editor.Quests
{
    public class CheckpointsEditorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset;

        [MenuItem("Duality/Checkpoints", false, 0)]
        private static void ShowWindow()
        {
            var window = GetWindow<CheckpointsEditorWindow>();
            window.titleContent = new GUIContent("Checkpoints");
            window.Show();
        }

        private MultiColumnListView _checkpointList;

        private static readonly BindingId ToggleValueBindingId = new(nameof(Toggle.value));
        private static readonly BindingId LabelTextBindingId = new(nameof(Label.text));
        private static readonly Binding ReachedBinding = new DataBinding
        {
            dataSourcePath = new PropertyPath(nameof(Checkpoint.Reached)),
            bindingMode = BindingMode.TwoWay,
        };
        private static readonly Binding NameBinding = new DataBinding
        {
            dataSourcePath = new PropertyPath(nameof(Checkpoint.Name)),
            bindingMode = BindingMode.ToTarget,
        };

        private void OnEnable()
        {
            EditorApplication.projectChanged -= ReloadCheckpoints;
            EditorApplication.projectChanged += ReloadCheckpoints;
        }

        private void OnDisable()
        {
            EditorApplication.projectChanged -= ReloadCheckpoints;
        }

        private void ReloadCheckpoints()
        {
            Debug.Assert(_checkpointList != null);
            var checkpoints = AssetDatabase.FindAssetGUIDs("t:Checkpoint")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<Checkpoint>)
                .ToList();
            checkpoints.Sort((a,b) => string.CompareOrdinal(a.name, b.name));
            _checkpointList.itemsSource = checkpoints;
        }

        private void RebuildEditorWindow()
        {
            rootVisualElement.Add(_visualTreeAsset.CloneTree());
            _checkpointList = rootVisualElement.Q<MultiColumnListView>("Checkpoints");
            _checkpointList.columns[0].makeCell = () =>
            {
                var label = new Label();
                label.SetBinding(LabelTextBindingId, NameBinding);
                return label;
            };
            _checkpointList.columns[1].makeCell = () =>
            {
                var reachedToggle = new Toggle();
                reachedToggle.SetBinding(ToggleValueBindingId, ReachedBinding);
                return reachedToggle;
            };
        }

        private void CreateGUI()
        {
            RebuildEditorWindow();
            ReloadCheckpoints();
        }
    }
}
