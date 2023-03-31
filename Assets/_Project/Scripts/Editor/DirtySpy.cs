using UnityEditor;
using UnityEngine;

namespace DualityGame.Editor
{
    public class DirtySpy : EditorWindow
    {
        [MenuItem("MENUITEM/MENUITEMCOMMAND")]
        private static void ShowWindow()
        {
            var window = GetWindow<DirtySpy>();
            window.titleContent = new GUIContent("TITLE");
            window.Show();
        }

        private void OnGUI()
        {
            
        }
    }
}