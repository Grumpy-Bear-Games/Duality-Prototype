using DualityGame.SaveSystem;
using DualityGame.VFX;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameSession _gameSession;
        [SerializeField] private ScreenFader _screenFader;

        private VisualElement _frame;

        private SettingsMenu _settingsMenu;
        private void Awake()
        {
            _settingsMenu = GetComponent<SettingsMenu>();

            var root = GetComponent<UIDocument>().rootVisualElement;

            _frame = root.Q<VisualElement>("MainMenu");
            _frame.Q<Button>("NewGameButton").clicked += () =>
            {
                CoroutineRunner.Run(_screenFader.Wrap(_gameSession.NewGame()));
            };
            _frame.Q<Button>("SettingsButton").clicked += () =>
            {
                Hide();
                _settingsMenu.Show();
            };
            var continueButton = _frame.Q<Button>("ContinueButton");
            continueButton.clicked += () => CoroutineRunner.Run(_screenFader.Wrap(_gameSession.LoadGame()));
            if (!_gameSession.SavefileExists)
            {
                continueButton.style.display = DisplayStyle.None;
            }

            var confirmationDialog = root.Q<ConfirmationDialog>();
            confirmationDialog.Hide();
            confirmationDialog.OnConfirm += ExitGame;
            confirmationDialog.OnCancel += () =>
            {
                _frame.Query<Button>().ForEach(b => b.focusable = true);
                _frame.Q<Button>("QuitButton").Focus();
            };

            _frame.Q<Button>("QuitButton").clicked += () =>
            {
                _frame.Query<Button>().ForEach(b=> b.focusable = false);
                confirmationDialog.Show();
            };
        }

        private void Start() => _frame.Q<Button>("NewGameButton").Focus();

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
