using DualityGame.Core;
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

            var continueButton = _frame.Q<Button>("ContinueButton");
            continueButton.clicked += () => CoroutineRunner.Run(_screenFader.Wrap(_gameSession.LoadGame()));
            if (!_gameSession.HasSaveFile)
            {
                continueButton.style.display = DisplayStyle.None;
            }
            
            _frame.Q<Button>("NewGameButton").clicked += () =>
            {
                CoroutineRunner.Run(_screenFader.Wrap(_gameSession.NewGame()));
            };

            _settingsMenu.OnHide += () =>
            {
                Show();
                FocusHelper.Pop();
            };

            _frame.Q<Button>("SettingsButton").clicked += () =>
            {
                FocusHelper.Push();
                Hide();
                _settingsMenu.Show();
            };

            var confirmationDialog = root.Q<ConfirmationDialog>();
            confirmationDialog.Hide();
            confirmationDialog.OnConfirm += ExitGame;

            _frame.Q<Button>("QuitButton").clicked += confirmationDialog.Show;
            _frame.RegisterCallback<NavigationCancelEvent>(_ => confirmationDialog.Show());
        }

        private void Start() => _frame.Q<Button>(_gameSession.HasSaveFile ? "ContinueButton" : "NewGameButton").Focus();

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
