using Games.GrumpyBear.LevelManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class MainMenu : MonoBehaviour
    {
        [Header("New Game")]
        [SerializeField] private SceneGroup _firstLocation;

        private VisualElement _frame;

        private SettingsMenu _settingsMenu;
        private void Awake()
        {
            _settingsMenu = GetComponent<SettingsMenu>();

            var root = GetComponent<UIDocument>().rootVisualElement;

            _frame = root.Q<VisualElement>("MainMenu");
            _frame.Q<Button>("NewGameButton").clicked += () => _firstLocation.Load();
            _frame.Q<Button>("SettingsButton").clicked += () =>
            {
                Hide();
                _settingsMenu.Show();
            };

            var confirmationDialog = root.Q<ConfirmationDialog>();
            confirmationDialog.Hide();
            confirmationDialog.OnConfirm += ExitGame;

            _frame.Q<Button>("QuitButton").clicked += () => confirmationDialog.Show();
        }

        public void Hide() => _frame.AddToClassList("Hide");

        public void Show() => _frame.RemoveFromClassList("Hide");

        private static void ExitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
            #else
            Application.Quit();
            #endif
        }
    }
}
