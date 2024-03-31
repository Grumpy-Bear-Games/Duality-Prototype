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

        private void ReloadCheckpoints()
        {
            Debug.Assert(_checkpointList != null);
            var checkpoints = Resources.FindObjectsOfTypeAll<Checkpoint>().ToList();
            _checkpointList.itemsSource = checkpoints;
        }

        private void RebuildEditorWindow()
        {
            rootVisualElement.Clear();
            _checkpointList = new MultiColumnListView();
            rootVisualElement.Add(_checkpointList);
            _checkpointList.columns.Add(new Column
            {
                name = "name",
                title = "Name",
                stretchable = true,
                makeCell = () =>
                {
                    var label = new Label();
                    label.SetBinding(LabelTextBindingId, NameBinding);
                    return label;
                },
                bindCell = BindCell,
            });
            _checkpointList.columns.Add(new Column
            {
                name = "reached",
                title = "Reached",
                minWidth = 100,
                makeCell = () =>
                {
                    var reachedToggle = new Toggle();
                    reachedToggle.SetEnabled(EditorApplication.isPlayingOrWillChangePlaymode);
                    reachedToggle.SetBinding(ToggleValueBindingId, ReachedBinding);
                    return reachedToggle;
                },
                bindCell = BindCell,
            });
        }

        private void BindCell(VisualElement element, int index) => element.dataSource = _checkpointList.itemsSource[index] as Checkpoint;

        private void CreateGUI()
        {
            RebuildEditorWindow();
            ReloadCheckpoints();
        }

        private void Update()
        {
            ReloadCheckpoints();
        }
    }
}
