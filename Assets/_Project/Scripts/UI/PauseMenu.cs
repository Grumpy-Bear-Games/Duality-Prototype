using DualityGame.Core;
using DualityGame.SaveSystem;
using DualityGame.VFX;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class PauseMenu: MonoBehaviour
    {
        [SerializeField] private UnityEvent _onContinue;
        [SerializeField] private GameSession _gameSession;
        [SerializeField] private SceneGroup _mainMenuSceneGroup;
        [SerializeField] private ScreenFader _screenFader;

        private VisualElement _frame;

        private SettingsMenu _settingsMenu;
        private VisualElement _root;

        private void Awake()
        {
            _settingsMenu = GetComponent<SettingsMenu>();

            _root = GetComponent<UIDocument>().rootVisualElement;

            _frame = _root.Q<VisualElement>("PauseMenu");

            _settingsMenu.OnHide += () =>
            {
                Show();
                FocusHelper.Pop();
            };

            _frame.Q<Button>("ContinueButton").clicked += Continue;
            _frame.Q<Button>("SettingsButton").clicked += Settings;
            _frame.Q<Button>("ExitToMainMenuButton").clicked += ExitToMainMenu;
            _frame.Q<Button>("QuitButton").clicked += ExitGame;
            _frame.RegisterCallback<NavigationCancelEvent>( _ => Continue());

            Hide();
        }

        private void Continue()
        {
            Hide();
            _onContinue.Invoke();
        }

        private void Settings()
        {
            FocusHelper.Push();
            Hide();
            _settingsMenu.Show();
        }

        private void ExitToMainMenu()
        {
            ConfirmationDialog.ShowConfirm(_root, "Return to main menu?", "OK", "Back", () =>
            {
                _gameSession.SaveGame();
                CoroutineRunner.Run(_screenFader.Wrap(_mainMenuSceneGroup.Load_CO()));
            });
        }

        private void Hide() => _frame.AddToClassList("Hide");

        private void Show()
        {
            _frame.RemoveFromClassList("Hide");
            _frame.Q<Button>("ContinueButton").Focus();
        }

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

        private void OnEnable() => Show();

        private void OnDisable()
        {
            _settingsMenu.Hide();
            Hide();
        }
    }
}
