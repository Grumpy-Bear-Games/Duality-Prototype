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
        private VisualElement _root;

        private void Awake()
        {
            _settingsMenu = GetComponent<SettingsMenu>();

            _root = GetComponent<UIDocument>().rootVisualElement;

            _frame = _root.Q<VisualElement>("MainMenu");

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

            _frame.Q<Button>("QuitButton").clicked += ExitGame;
            _frame.RegisterCallback<NavigationCancelEvent>(_ => ExitGame());
        }

        private void Start() => _frame.Q<Button>(_gameSession.HasSaveFile ? "ContinueButton" : "NewGameButton").Focus();

        private void Hide() => _frame.RemoveFromClassList("Shown");

        private void Show() => _frame.AddToClassList("Shown");

        private void ExitGame()
        {
            ConfirmationDialog.ShowConfirm(_root, "Really Quit?", "Quit", "Back", () =>
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
                #else
                Application.Quit();
                #endif
            });
        }
    }
}
