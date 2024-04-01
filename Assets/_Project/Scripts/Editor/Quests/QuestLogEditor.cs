using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Editor.Quests
{
    [CustomEditor(typeof(DualityGame.Quests.QuestLog))]
    public class QuestLogEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset _runtimeInspectorTemplate;
        private DualityGame.Quests.QuestLog _questLog;
        private readonly List<DualityGame.Quests.Quest> _cachedQuests = new();
        private MultiColumnListView _questsListview;

        private void OnEnable()
        {
            _questLog = target as DualityGame.Quests.QuestLog;

            Debug.Assert(_questLog != null);

            _cachedQuests.Clear();
            foreach (var guid in AssetDatabase.FindAssets("t:Quest"))
            {
                var quest = AssetDatabase.LoadAssetAtPath<DualityGame.Quests.Quest>(AssetDatabase.GUIDToAssetPath(guid));
                Debug.Log(quest);
                _cachedQuests.Add(quest);
            }
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement
            {
                dataSource = _questLog
            };
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                root.Add(CreateRuntimeInspector());
            }

            return root;
        }

        private VisualElement CreateRuntimeInspector()
        {
            var root = _runtimeInspectorTemplate.CloneTree();
            _questsListview = root.Q<MultiColumnListView>();
            _questsListview.itemsSource = _questLog.Entries.ToList();
            _questsListview.bindingSourceSelectionMode = BindingSourceSelectionMode.AutoAssign;
            _questsListview.columns[0].makeCell = () => new Label();
            _questsListview.overridingAddButtonBehavior = OverridingAddButtonBehavior;

            return root;
        }

        private void OverridingAddButtonBehavior(BaseListView baseListView, Button addButton)
        {
            if (baseListView is not MultiColumnListView listview) return;
            var menu = new GenericDropdownMenu();
            foreach (var quest in _cachedQuests)
            {
                menu.AddItem(quest.name, false, () => _questLog.Add(quest));
            }
            menu.DropDown(addButton.worldBound, addButton);
        }
    }
}
