using System.Collections.Generic;
using System.Linq;
using DualityGame.Warp;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.Editor.Warp
{
    public class WarpShaderIssueFinder : EditorWindow
    {
        [MenuItem("Duality Game/Warp Shader Issue Finder")]
        private static void ShowWindow()
        {
            var window = GetWindow<WarpShaderIssueFinder>();
            window.titleContent = new GUIContent("Warp Shader Issue Finder");
            window.Show();
        }

        private ListView _listView;
        private Button _refreshButton;
        private static readonly int RealmProperty = Shader.PropertyToID("_Realm");
        private readonly List<GameObject> _issues = new();

        private void CreateGUI()
        {
            _listView = new ListView
            {
                makeItem = () => new Label()
                {
                    bindingPath = "m_Name"
                },
                bindItem = (e, idx) =>
                {
                    if (e is not Label label) return;
                    var root = PrefabUtility.GetNearestPrefabInstanceRoot(_issues[idx]);
                    label.text = $"{_issues[idx].name} ({root.name})";
                },
                itemsSource = _issues,
            };
            _listView.selectedIndicesChanged += (e) =>
            {
                var go = e.Select(i => _issues[i]).First();
                Selection.activeGameObject = go;
            };

            _refreshButton = new Button(Refresh)
            {
                text = "Refresh"
            };
            rootVisualElement.Add(_refreshButton);
            rootVisualElement.Add(_listView);
        }



        private void Refresh()
        {
            _issues.Clear();
            foreach (var renderer in FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None))
            {
                var material = renderer.sharedMaterial;
                if (material == null)
                {
                    continue;
                }

                if (!material.HasProperty(RealmProperty))
                {
                    continue;
                }
                var realm = (int)material.GetFloat(RealmProperty);
                if (realm == renderer.gameObject.layer) continue;
                _issues.Add(renderer.gameObject);
            }
            _listView.RefreshItems();
        }
    }
}
